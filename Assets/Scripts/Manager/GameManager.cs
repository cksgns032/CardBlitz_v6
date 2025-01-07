using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : SingleTon<GameManager>
{
    // 유저의 카드 정보를 가져와서 랜덤한 5장을 선별 배포
    // 게임의 타이머 조절 
    // 게임 결과 나타냄
    GameMap map;
    GameUI gameUI;
    Fade fade;

    List<Player> myList = new List<Player>();// 아군 오브젝트 리스트
    List<Player> enemyList = new List<Player>();// 적군 오브젝트 리스트

    bool isClaer = false;
    float timeNum = 100;
    float cardTime = 3;
    float gaugeTime = 3;

    void Start()
    {
        // 매니저 초기화
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "BattleSound");
        AudioManager.Instance.PlayBgm(true, "BattleSound");
        PoolingManager.Instance.Init();
        // 기본적 프리펩 초기화
        fade = GameObject.FindAnyObjectByType<Fade>();
        if (fade != null)
        {
            fade.FadeIn();
        }
        gameUI = GameObject.FindAnyObjectByType<GameUI>();
        if(gameUI)
        {
            gameUI.Init();
            gameUI.UpdateTime(timeNum);
        }
        GameObject mapPos = GameObject.Find("MapPos");
        GameObject mapObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Map"), mapPos.GetComponent<Transform>());
        map = mapObj.GetComponent<GameMap>();
        map.Init();
        CardFill();
    }
    public GameMap GetGameMap()
    {
        if(map == null)
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
        if (isClaer == false && timeNum > 0)
        {
            timeNum -= Time.deltaTime;
            gameUI.UpdateTime(timeNum);
        }
        else if (isClaer == false && timeNum <= 0)
        {
            isClaer = true;
            gameUI.Result(RESULT.DRAW);
        }
    }
    // 스폰위치에 유닛 소환
    public void CreateHero(string objTag, string objName, Team team, int useCost)
    {
        Player monObj = Instantiate<Player>(Resources.Load<Player>("Prefabs/Monster/" + objName));
        switch (objTag)
        {
            case "TOP":
                Vector3 pos = map.transform.Find("SpawnPos/TopPotal").transform.position;
                monObj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
            case "MIDDLE":
                pos = map.transform.Find("SpawnPos/MiddlePotal").transform.position;
                monObj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
            case "BOTTOM":
                pos = map.transform.Find("SpawnPos/BottomPotal").transform.position;
                monObj.gameObject.transform.position = new Vector3(pos.x, 1.2f, pos.z);
                break;
        }
        monObj.gameObject.layer = LayerMask.NameToLayer("HERO");
        monObj.gameObject.tag = "Player";
        monObj.Init(team);
        monObj.AgentMaskSet(objTag, team);
        // 오브젝트의 위치, 정보를 보냄
        //TCPClient.Instance.CreateObj(MonName, tagName);

        // 내 팀이 소환을 했을 때
        if (team == UserData.team)
        {
            myList.Add(monObj);
        }
        // 적 팀이 소환을 했을 때
        else
        {
            enemyList.Add(monObj);
        }
        gameUI.UseCost(team, -useCost);
    }
    public List<Player> GetMyList()
    {
        return myList;
    }
    public List<Player> GetEnemyList()
    {
        return enemyList;
    }
    #region 게임 결과 
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
    #endregion 게임 결과

    #region 메뉴 쪽으로 토스
    public void UpdateHp(Team hitTeam,float attack)
    {
        gameUI.UpdateTower(hitTeam,attack);
    }
    #endregion 메뉴 쪽으로 토스
    #region 게이지 추가
    public void GaugeFill()
    {
        StartCoroutine(GaugeFill(gaugeTime));
    }
    IEnumerator GaugeFill(float cardTime)
    {
        while (GameManager.Instance.GetClear() == false)
        {
            yield return new WaitForSeconds(cardTime);
            gameUI.CargeGauge();
        }
    }
    #endregion 게이지 추가
    #region 카드 추가
    public void CardFill()
    {
        StartCoroutine(CardFill(cardTime));
    }
    IEnumerator CardFill(float delayTime)
    {
        while (GameManager.Instance.GetClear() == false)
        {
            yield return new WaitForSeconds(delayTime);
            gameUI.CargeCard();
        }
    }
    #endregion 카드 추가
    #region 게임 버프
    // 이속 증가 버프
    public void LastBuffe()
    {
        cardTime = 1.5f;
        gaugeTime = 1.5f;

        List<Player> allObject = new List<Player>();
        allObject.AddRange(myList);
        allObject.AddRange(enemyList);

        for (int i = 0; i < allObject.Count; i++)
        {
            if (allObject[i].GetComponent<Player>())
            {
                BuffData data = new BuffData();
                allObject[i].AddBuff(data);
            }
        }
    }
    #endregion 게임 버프
}