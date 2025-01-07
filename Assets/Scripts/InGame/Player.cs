using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    GameObject damageObj;

    //CapsuleCollider colder;
    NavMeshAgent agent;
    Rigidbody rigid;
    Animator ani;
    [SerializeField] PlayerAttackRange attackRangeCom ;
    [SerializeField] Collider[] enemyList;
    EventButton btnEvent;
    [SerializeField] HeroData info = new HeroData();
    [SerializeField] List<Buff> buffList = new List<Buff>();

    bool isDie = false;// 생 사 여부
    bool isCharge = false;// 점령 여부

    public void Init(Team team)
    {
        damageObj = transform.Find("DamagePos").gameObject;
        attackRangeCom = GetComponentInChildren<PlayerAttackRange>(true);

        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.enabled = false;
        }
        ani = GetComponentInChildren<Animator>();
        if (ani != null)
        {
            ani.enabled = true;
        }
        rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.isKinematic = true;
        }

        // todo : 내 몬스터 이름을 가지고 몬스터 테이블 읽어서 능려치 적용
        SetStat();
    }
    public void SetTriggerLyaer()
    {
        // layer trigger Controll
        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            if (!string.IsNullOrEmpty(layerName)) // 비어 있지 않은 레이어만 출력
            {
                if(layerName == "HERO")
                {
                    continue;
                }
                bool isTrigger = true;
                if (layerName == "ENEMY")
                {
                    isTrigger = false;
                }
                int myLayer = LayerMask.NameToLayer("HERO");
                int otherLayer = LayerMask.NameToLayer(layerName);
                Physics.IgnoreLayerCollision(myLayer, otherLayer, isTrigger);
            }
        }
    }
    public void ReMoveBuff(Buff buff)
    {
        foreach(Buff bf in buffList)
        {
            if(buff == bf)
            {
                buffList.Remove(bf);
                break;
            }
        }
    }
    public void AddBuff(BuffData data)
    {
        Buff buff = new Buff();
        buff.SetBuff(data, this);
        buffList.Add(buff);
    }
    public void SetStat()
    {
        info.hp = 100;
        info.attack = 100;
        info.defence = 1;
        info.attackCnt = 1;
        info.attackRange = 10;
        info.moveSpeed = 1f;
        agent.speed = info.moveSpeed;
    }
    // 라인 정하는 함수
    public void AgentMaskSet(string type,Team team)
    {
        int areaNum;
        switch (type)
        {
            case "TOP":
                areaNum = NavMesh.GetAreaFromName("TOP");
                agent.areaMask = 1 << areaNum;
                if(UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("TOPSPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("ETOPSPAWN").transform.position;
                break;
            case "MIDDLE":
                areaNum = NavMesh.GetAreaFromName("MIDDLE");
                agent.areaMask = 1 << areaNum;
                if (UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("MIDDLESPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("EMIDDLESPAWN").transform.position;

                break;
            case "BOTTOM":
                areaNum = NavMesh.GetAreaFromName("BOTTOM");
                agent.areaMask = 1 << areaNum;
                if (UserData.team == team)
                    transform.position = GameObject.FindGameObjectWithTag("BOTTOMSPAWN").transform.position;
                else
                    transform.position = GameObject.FindGameObjectWithTag("EBOTTOMSPAWN").transform.position;
                break;
        }
        agent.enabled = true;
        rigid.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear())
            return;
        if (other.gameObject.tag == "EVENTBUTTON")
        {
            btnEvent = other.gameObject.GetComponent<EventButton>();
            // 차징 중인지 이미 내 팀이 점령을 했는지
            if(btnEvent.CheckState() == false && btnEvent.GetColor() != UserData.team)
            {
                isCharge = true;
                btnEvent.Charging(isCharge);
                if (agent)
                {
                    agent.isStopped = isCharge;
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.GetClear())
            return;
        // 버튼 점령
        if (other.gameObject.tag == "EVENTBUTTON" && isCharge)
        {
            EventButton eventCom = other.gameObject.GetComponent<EventButton>();
            if (eventCom != null)
            {
                if (eventCom.GetColor() != UserData.team)
                {                 
                    if (eventCom.ChargeImage(1 * Time.deltaTime, LayerMask.LayerToName(gameObject.layer)))
                    {
                        isCharge = false;
                        Go();
                    }
                }
            }
        }
    }
    public void Attack()
    {
        if (GameManager.Instance.GetClear())
            return;
        Debug.Log("Attack");
        // 가까운 순으로 리스트를 정렬 후 
        //enemyList.Sort(EnemySort);
        // 공격 수 만큼 for문을 돌려줘야함
        for(int i = 0; i < enemyList.Length; i++)
        {
            if (i >= info.attackCnt)
                return;
            else
            {
                Debug.Log(enemyList[0].gameObject.name);
                // 성
                if (enemyList[i].gameObject.tag == "EnemyTower")
                {
                    Team hitTeam = Team.Red == UserData.team ? Team.Blue : Team.Red;
                    GameManager.Instance.UpdateHp(hitTeam,info.attack);
                }
                // 몬스터
                else if (enemyList[i].gameObject.tag == "Player")
                {
                    if (enemyList[i].GetComponent<Player>().isDie == false)
                        enemyList[i].GetComponent<Player>().Damage(info.attack);
                }
            }
        }
        /*for (int i = 0; i < info.attackCnt; i++)
        {
            Debug.Log(enemyList[0].gameObject.name);
            // 성
            if (enemyList[i].gameObject.tag == "EnemyTower")
            {
               GameManager.Instance.UpdateHp(info.attack);
            }
            // 몬스터
            else if(enemyList[i].gameObject.tag == "Player")
            {
                if (enemyList[i].GetComponent<Player>().isDie == false)
                    enemyList[i].GetComponent<Player>().Damage(info.attack);
            }
        }*/
    }   
    int EnemySort(GameObject left, GameObject right)
    {
        float leftDis = Vector3.Distance(this.gameObject.transform.position, left.transform.position);
        float rightDis = Vector3.Distance(this.gameObject.transform.position, right.transform.position);
        if (leftDis > rightDis)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
    public void Damage(float damage)
    {
        GameObject obj = PoolingManager.Instance.Pool.Get();
        DamageTxt txt = obj.GetComponent<DamageTxt>();
        if(txt != null)
        {
            txt.Setting(damageObj.transform.position,damage);
        }
        // 자기의 피가 닳아야 된다 damage만큼
        if(info.hp - damage > 0)
        {
            info.hp -= (int)damage;
            ani.SetTrigger("Hit");
        }
        // 만약 피가 0이하면 
        else
        {
            Die();
        }
    }
    public void Die()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        isDie = true;
        for(int i = 0; i < enemyList.Length; i++)
        {
            /*if (enemyList[i] == gameObject)
            {
                enemyList.RemoveAt(i);
            }*/
        }
        if (isCharge)
        {
            isCharge = false;
            btnEvent.Charging(false);
        }
        ani.SetTrigger("Death");
    }
    void Go()
    {
        if (gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            return;
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
    }
    public bool GetState()
    {
        return isDie;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        if (isDie == true || GameManager.Instance.GetClear())
            return;
        // 이동 목표 설정
        if (agent.hasPath == false && agent.enabled == true)
        {
            if (gameObject.layer == LayerMask.NameToLayer("HERO"))
            {
                GameObject obj = GameObject.Find("EnemyGoal");
                agent.SetDestination(obj.transform.position);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("ENEMY"))
            {
                GameObject obj = GameObject.Find("MyGoal");
                agent.SetDestination(obj.transform.position);
            }
        }

        // 적 찾기
        /*LayerMask layerNum = LayerMask.GetMask("ENEMY");
        enemyList = Physics.OverlapSphere(transform.position, info.attackRange, layerNum);
        if (agent.enabled == true && enemyList.Length > 0 && isCharge == false)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ani.SetTrigger("Attack");
        }*/
    }
    // test
    /*private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, info.attackRange);
    }*/
}
