using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    GameObject damageObj;

    //CapsuleCollider colder;
    NavMeshAgent agent;
    Rigidbody rigid;
    Animator ani;
    [SerializeField] PlayerAttackRange attackRangeCom;
    EventButton btnEvent;
    [SerializeField] HeroData info = new HeroData();
    [SerializeField] List<Buff> buffList = new List<Buff>();
    [SerializeField] List<Player> enemyList = new List<Player>();

    bool isDie = false;
    bool isCharge = false;
    WaitForSeconds waitAttack;
    Coroutine attackCoroutine;
    float attackDelay;
    public void Init()
    {
        damageObj = transform.Find("DamagePos").gameObject;
        attackRangeCom = GetComponentInChildren<PlayerAttackRange>(true);
        if (attackRangeCom)
        {
            attackRangeCom.Init();
        }

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

        // todo : �� ���� �̸��� ������ ���� ���̺� �о �ɷ�ġ ����
        SetStat();
    }
    public void SetTarget()
    {
        // 목표 지점 설정 및 이동
        if (agent.enabled == true)
        {
            if (enemyList.Count > 0)
            {
                agent.SetDestination(enemyList[0].transform.position);
            }
            else
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
        }
    }
    public void SetStat()
    {
        info.hp = 100;
        info.defence = 1;
        info.attack = 100;
        info.attackSpeed = 5;
        waitAttack = new WaitForSeconds(info.attackSpeed);
        info.attackCnt = 1;
        info.attackRange = 10;
        info.moveSpeed = 1f;
        attackDelay = 0;
        agent.speed = info.moveSpeed;
        agent.stoppingDistance = info.attackRange;
        ani.SetFloat("Blend", agent.speed);
    }
    public void AgentMaskSet(string type, Team team)
    {
        int areaNum;
        switch (type)
        {
            case "TOP":
                areaNum = NavMesh.GetAreaFromName("TOP");
                agent.areaMask = 1 << areaNum;
                if (UserData.team == team)
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
        SetTarget();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear())
            return;
        if (other.gameObject.tag == "EVENTBUTTON")
        {
            btnEvent = other.gameObject.GetComponent<EventButton>();
            // ��¡ ������ �̹� �� ���� ������ �ߴ���
            if (btnEvent.CheckState() == false && btnEvent.GetColor() != UserData.team)
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
        // 우선순위 
        // 유닛 공격 < 버튼 점령 

        // 이벤트 버튼 점령
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
        // 유닛 공격
        if (!isCharge && enemyList.Count > 0)
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(IEAttack());
            }
        }
    }
    #region enemyList
    public bool CheckList(Player player)
    {
        foreach (Player data in enemyList)
        {
            if (data == player)
            {
                return false;
            }
        }
        return true;
    }
    int EnemySort(Player left, Player right)
    {
        float leftDis = Vector3.Distance(this.gameObject.transform.position, left.transform.position);
        float rightDis = Vector3.Distance(this.gameObject.transform.position, right.transform.position);
        if (leftDis > rightDis)
        {
            return -1;
        }
        else if (leftDis == rightDis)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    public void AddEemyList(Player enemyPlayer)
    {
        if (CheckList(enemyPlayer))
        {
            enemyList.Add(enemyPlayer);
            // 내 위치 기준으로 sort
            enemyList.Sort(EnemySort);
            SetTarget();
        }
    }
    public void RemoveEnemyList(Player enemyPlayer)
    {
        foreach (Player player in enemyList)
        {
            if (player == enemyPlayer)
            {
                enemyList.Remove(player);
                SetTarget();
                break;
            }
        }
    }
    #endregion enemyList
    #region attack
    IEnumerator IEAttack()
    {
        if (GameManager.Instance.GetClear())
        {
            StopCoroutine(attackCoroutine);
            yield return null;
        }
        while (enemyList.Count > 0)
        {
            Attack();
            yield return waitAttack;
        }
    }
    public void Attack()
    {
        if (GameManager.Instance.GetClear())
            return;
        if (attackDelay > info.attackSpeed)
        {
            // 공격 가능 유닛체크 공격
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (i >= info.attackCnt)
                    return;
                else
                {
                    Debug.Log("real Attack");
                    ani.SetTrigger("Attack");
                    // 타워 공격
                    if (enemyList[i].gameObject.tag == "EnemyTower")
                    {
                        Team hitTeam = Team.Red == UserData.team ? Team.Blue : Team.Red;
                        GameManager.Instance.UpdateHp(hitTeam, info.attack);
                    }
                    // 몬스터 공격
                    else if (enemyList[i].gameObject.tag == "Player")
                    {
                        if (enemyList[i].GetComponent<Player>().isDie == false)
                        {
                            //enemyList[i].GetComponent<Player>().Damage(info.attack);
                        }
                    }
                    attackDelay = 0;
                }
            }
        }
    }
    public void Damage(float damage)
    {
        GameObject obj = PoolingManager.Instance.Pool.Get();
        DamageTxt txt = obj.GetComponent<DamageTxt>();
        if (txt != null)
        {
            txt.Setting(damageObj.transform.position, damage);
        }
        // 죽는지 확인
        if (info.hp - damage > 0)
        {
            info.hp -= (int)damage;
            ani.SetTrigger("Hit");
        }
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
        if (isCharge)
        {
            isCharge = false;
            btnEvent.Charging(false);
        }
        ani.SetTrigger("Death");
    }
    #endregion attack
    #region buff
    public void ReMoveBuff(Buff buff)
    {
        foreach (Buff bf in buffList)
        {
            if (buff == bf)
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
    #endregion buff
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

        attackDelay += Time.deltaTime;
    }
    // test
    /*private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, info.attackRange);
    }*/
}
