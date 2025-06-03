using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class GameManager : SingleTon<GameManager>
{
    GameMap map;
    GameUI gameUI;
    FadeUI fade;

    List<Monster> myList = new List<Monster>();
    List<Monster> enemyList = new List<Monster>();
    private UserGameData myGameData = new UserGameData();

    bool isClaer = false;
    float gameTime = 100;
    float cardTime = 3;
    float gaugeTime = 3;

    // todo : ai test용

    void Start()
    {
        // todo : 하드 코딩 > 서버 데이터 변경
        myGameData.Init();
        // Audio
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "BattleSound");
        AudioManager.Instance.PlayBgm(true, "BattleSound");
        // Pool
        PoolingManager.Instance.Init();
        // Fade
        fade = GameObject.FindAnyObjectByType<FadeUI>();
        if (fade != null)
        {
            fade.FadeIn();
        }
        // UI
        gameUI = GameObject.FindAnyObjectByType<GameUI>();
        if (gameUI)
        {
            gameUI.Init();
        }
        // Map
        GameObject mapPos = GameObject.Find("MapPos");
        GameObject mapObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Map/Map"), mapPos.GetComponent<Transform>());
        map = mapObj.GetComponent<GameMap>();
        map.Init();
    }
    public UserGameData GetMyGameData()
    {
        return myGameData;
    }
    public GameMap GetGameMap()
    {
        if (map == null)
        {
            GameObject mapPos = GameObject.Find("MapPos");
            map = mapPos.GetComponentInChildren<GameMap>();
        }
        return map;
    }
    private void Update()
    {
        if (isClaer)
            return;
        if (isClaer == false && gameTime > 0)
        {
            gameTime -= Time.deltaTime;
            gameUI.UpdateTime(gameTime);
        }
        else if (isClaer == false && gameTime <= 0)
        {
            isClaer = true;
            gameUI.Result(RESULT.DRAW);
        }
    }
    void FixedUpdate()
    {
        // 주기적으로 유닛의 위치를 보냄
        List<Monster> totalMonsterList = enemyList.Concat<Monster>(myList).ToList();
        List<UnitPositionData> monPosDataList = new List<UnitPositionData>();
        foreach (Monster obj in totalMonsterList)
        {
            double posX = Math.Round(obj.gameObject.transform.position.x * 100);
            double posY = Math.Round(obj.gameObject.transform.position.y * 100);
            UnitPositionData data = new UnitPositionData() { UnitId = 1, PosX = (short)posX, PosY = (short)posY };
            monPosDataList.Add(data);
        }
        SendBatchedUnitPositions(monPosDataList);
    }
    public byte[] SerializePosition(UnitPositionData data)
    {
        byte[] buffer = new byte[6]; // 데이터 크기
        Buffer.BlockCopy(BitConverter.GetBytes(data.UnitId), 0, buffer, 0, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(data.PosX), 0, buffer, 2, 2);
        Buffer.BlockCopy(BitConverter.GetBytes(data.PosY), 0, buffer, 4, 2);
        return buffer;
    }
    public void SendBatchedUnitPositions(List<UnitPositionData> positions)
    {
        if (positions == null || positions.Count == 0) return;

        // 메시지 타입 (1 byte) + 유닛 개수 (1 byte, 최대 255개 유닛 가정)
        int headerSize = 2;
        int dataPerUnitSize = 6; // UnitPositionData 크기
        int totalDataSize = headerSize + positions.Count * dataPerUnitSize;
        byte[] batchedData = new byte[totalDataSize];

        batchedData[0] = 0x01; // 메시지 타입: 유닛 위치 배치 업데이트
        batchedData[1] = (byte)positions.Count;

        int offset = headerSize;
        foreach (var posData in positions)
        {
            byte[] unitDataBytes = SerializePosition(posData); // 위에서 정의한 직렬화 함수
            Buffer.BlockCopy(unitDataBytes, 0, batchedData, offset, unitDataBytes.Length);
            offset += unitDataBytes.Length;
        }

        TestTCP.Instance.SendMessageToServer(batchedData);
    }
    // 몬스터 생성(todo : 몬스터 풀링을 만들어서 거기서 관리)
    public void CreateHero(string objTag, string objName, Team team, int useCost)
    {
        // todo : 서버에 생성 패킷 보냄

        // 딕셔너리 체크
        if (PoolingManager.Instance.MonsterPoolList.TryGetValue(objName, out IObjectPool<GameObject> obj) == false)
        {
            PoolingManager.Instance.SetPool(objName);
        }
        GameObject poolObj = PoolingManager.Instance.MonsterPoolList[objName].Get();
        Monster monCom = poolObj.GetComponent<Monster>();
        monCom.Init();
        monCom.AgentMaskSet(objTag, team);

        // todo : 서버 활성화 후
        //TCPClient.Instance.CreateObj(MonName, tagName);

        if (team == myGameData.team)
        {
            myList.Add(monCom);
            monCom.SetUniqueID((short)myList.Count);
        }
        else
        {
            enemyList.Add(monCom);
            monCom.SetUniqueID((short)enemyList.Count);
        }
        gameUI.UseCost(team, -useCost);
    }
    public List<Monster> GetMyList()
    {
        return myList;
    }
    public List<Monster> GetEnemyList()
    {
        return enemyList;
    }

    #region Game Result
    public void ResultGame(RESULT result)
    {
        for (int i = 0; i < myList.Count; i++)
        {
            if (myList[i].GetComponent<NavMeshAgent>())
                myList[i].GetComponent<NavMeshAgent>().isStopped = true;
        }
        for (int j = 0; j < enemyList.Count; j++)
        {
            if (enemyList[j].GetComponent<NavMeshAgent>())
                enemyList[j].GetComponent<NavMeshAgent>().isStopped = true;
        }
        gameUI.Result(result);
    }
    public void SetClear(bool state)
    {
        isClaer = state;
    }
    public bool GetClear()
    {
        return isClaer;
    }
    #endregion Game Result

    #region Game UI
    public GameUI GetGameUI()
    {
        return gameUI;
    }
    #endregion Game UI

    #region Fever Time
    // �̼� ���� ����
    public void FeverTime()
    {
        cardTime = 1.5f;
        gaugeTime = 1.5f;

        List<Monster> allObject = new List<Monster>();
        allObject.AddRange(myList);
        allObject.AddRange(enemyList);

        for (int i = 0; i < allObject.Count; i++)
        {
            if (allObject[i].GetComponent<Monster>())
            {
                BuffData data = new BuffData();
                allObject[i].AddBuff(data);
            }
        }
    }
    #endregion Fever Time
    #region Any Time
    public float GetCardTime()
    {
        return cardTime;
    }
    public float GetGaugeTime()
    {
        return gaugeTime;
    }
    public float GetTimer()
    {
        return gameTime;
    }
    #endregion Any Time
}