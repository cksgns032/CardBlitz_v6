using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class GameManager : SingleTon<GameManager>
{
    GameMap map;
    GameUI gameUI;
    Fade fade;

    List<Monster> myList = new List<Monster>();
    List<Monster> enemyList = new List<Monster>();
    private UserGameData myGameData = new UserGameData();
    private UserGameData otherGameData = new UserGameData();

    bool isClaer = false;
    float gameTime = 100;
    float cardTime = 3;
    float gaugeTime = 3;

    // todo : ai test용

    void Start()
    {
        // todo : 하드 코딩 > 서버 데이터 변경
        myGameData.Init();
        otherGameData.Init();
        // Audio
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "BattleSound");
        AudioManager.Instance.PlayBgm(true, "BattleSound");
        // Pool
        PoolingManager.Instance.Init();
        // Fade
        fade = GameObject.FindAnyObjectByType<Fade>();
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
    public UserGameData GetOtherGameData()
    {
        return otherGameData;
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
    // 몬스터 생성(todo : 몬스터 풀링을 만들어서 거기서 관리)
    public void CreateHero(string objTag, string objName, Team team, int useCost)
    {
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
        }
        else
        {
            enemyList.Add(monCom);
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