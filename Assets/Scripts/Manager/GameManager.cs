using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : SingleTon<GameManager>
{
    // ������ ī�� ������ �����ͼ� ������ 5���� ���� ����
    // ������ Ÿ�̸� ���� 
    // ���� ��� ��Ÿ��
    GameMap map;
    GameUI gameUI;
    Fade fade;

    List<Player> myList = new List<Player>();// �Ʊ� ������Ʈ ����Ʈ
    List<Player> enemyList = new List<Player>();// ���� ������Ʈ ����Ʈ

    bool isClaer = false;
    float timeNum = 100;
    float cardTime = 3;
    float gaugeTime = 3;

    void Start()
    {
        // �Ŵ��� �ʱ�ȭ
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "BattleSound");
        AudioManager.Instance.PlayBgm(true, "BattleSound");
        PoolingManager.Instance.Init();
        // �⺻�� ������ �ʱ�ȭ
        fade = GameObject.FindAnyObjectByType<Fade>();
        if (fade != null)
        {
            fade.FadeIn();
        }
        gameUI = GameObject.FindAnyObjectByType<GameUI>();
        if (gameUI)
        {
            gameUI.Init();
        }
        GameObject mapPos = GameObject.Find("MapPos");
        GameObject mapObj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Map"), mapPos.GetComponent<Transform>());
        map = mapObj.GetComponent<GameMap>();
        map.Init();
        CardFill();
        GaugeFill();
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
    // ������ġ�� ���� ��ȯ
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
        monObj.Init();
        monObj.AgentMaskSet(objTag, team);
        // ������Ʈ�� ��ġ, ������ ����
        //TCPClient.Instance.CreateObj(MonName, tagName);

        // �� ���� ��ȯ�� ���� ��
        if (team == UserData.team)
        {
            myList.Add(monObj);
        }
        // �� ���� ��ȯ�� ���� ��
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
    #region ���� ��� 
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
    #endregion ���� ���

    #region �޴� ������ �佺
    public void UpdateHp(Team hitTeam, float attack)
    {
        gameUI.UpdateTower(hitTeam, attack);
    }
    #endregion �޴� ������ �佺
    #region ������ �߰�
    public void GaugeFill()
    {
        StartCoroutine(GaugeFill(gaugeTime));
    }
    IEnumerator GaugeFill(float cardTime)
    {
        while (GameManager.Instance.GetClear() == false)
        {
            yield return new WaitForSeconds(cardTime);
            gameUI.ChargeGauge();
        }
    }
    #endregion ������ �߰�
    #region ī�� �߰�
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
    #endregion ī�� �߰�
    #region ���� ����
    // �̼� ���� ����
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
    #endregion ���� ����
}