using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    // component
    NavMeshAgent agent;
    Rigidbody rigid;
    Animator ani;
    // hero
    EventButton btnEvent;
    [SerializeField] HeroData info = new HeroData();
    PlayerState stateCom;
    // attack
    [SerializeField] PlayerAttackRange attackRangeCom;
    Coroutine attackCoroutine;
    [SerializeField] List<Player> enemyList = new List<Player>();
    [SerializeField] List<Buff> buffList = new List<Buff>();
    bool isDie = false;
    bool isTest = false;

    public void Init()
    {
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
        stateCom = new PlayerState();
        if (stateCom != null)
        {
            stateCom.Init(this);
        }
        attackRangeCom = GetComponentInChildren<PlayerAttackRange>(true);
        if (attackRangeCom)
        {
            attackRangeCom.Init();
        }
    }
    public void TestInit()
    {
        isTest = true;
        agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.enabled = true;
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

        SetStat();
        stateCom = new PlayerState();
        if (stateCom != null)
        {
            stateCom.Init(this);
        }
        attackRangeCom = GetComponentInChildren<PlayerAttackRange>(true);
        if (attackRangeCom)
        {
            attackRangeCom.gameObject.SetActive(false);
        }
    }
    public List<Player> GetEnemyList()
    {
        return enemyList;
    }
    public void SetEnemyList(List<Player> enemyList){
        this.enemyList = enemyList;
    }
    public void SetStat()
    {
        info.hp = 100;
        info.defence = 1;
        info.attack = 0;
        info.attackSpeed = 5;
        info.attackSpeed = 10;
        info.attackCnt = 1;
        info.attackRange = 10;
        agent.stoppingDistance = info.attackRange;
        info.moveSpeed = isTest ? 0f : 1f;
        agent.speed = info.moveSpeed;
        agent.stoppingDistance = info.attackRange;
        ani.SetFloat("Blend", agent.speed);
    }
    public HeroData GetStat()
    {
        return info;
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
        stateCom.TransState(StateType.Move);
    }
    // 중복체크
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
            if(attackCoroutine == null)
            {
                AttackCoolTime();
            }
        }
    }
    public void RemoveEnemyList(Player enemyPlayer)
    {
        foreach (Player player in enemyList)
        {
            if (player == enemyPlayer)
            {
                enemyList.Remove(player);
                stateCom.TransState(StateType.Move);
                break;
            }
        }
    }
    public void Hit(float damage)
    {
        if (stateCom != null)
        {
            stateCom.Damage(damage);
        }
    }
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
        Debug.Log(buffList);
    }
    public void AddBuff(BuffData data)
    {
        Buff buff = new Buff();
        buff.SetBuff(data, this);
        buffList.Add(buff);
        Debug.Log(buffList);
    }
    #endregion buff
    public void SetEventButton(EventButton button)
    {
        btnEvent = button;
    }
    public EventButton GetEventButton()
    {
        return btnEvent;
    }
    public void SetDie(bool value)
    {
        isDie = value;
    }
    public bool IsDie()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        return isDie;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public PlayerState GetState()
    {
        return stateCom;
    }
    public void AttackCoolTime()
    {
        if (info.attackSpeed >= 0)
        {
            attackCoroutine = StartCoroutine(IEAttackCoolTime());
        }
    }
    IEnumerator IEAttackCoolTime()
    {
        float elapsedTime = 0f;
        float buffAttackSpeed = 0f;
        List<Buff> attackSpeedBuffList = buffList.Where(x => x.attackSpeed > 0).ToList();
        foreach (var buff in attackSpeedBuffList)
        {
            buffAttackSpeed += info.attackSpeed * buff.attackSpeed;
        }
        while (elapsedTime < Math.Max(0, info.attackSpeed - buffAttackSpeed))
        {
            elapsedTime += Time.deltaTime;
        }
        if (enemyList.Count > 0)
        {
            stateCom.TransState(StateType.Attack);
        }
        else 
        {
            stateCom.TransState(StateType.Move);
        }
        yield return null;
    }
    public void Update()
    {
        stateCom.Update();
    }
    // public void EditorGizmo(Transform transform)

    // {
    //     Color c = new Color(0, 0, 0.7f, 0.4f);

    //     UnityEditor.Handles.color = c;

    //     Vector3 rotatedForward = Quaternion.Euler(0, -270 * 0.5f, 0) * transform.forward;
    //     UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, 270, 10);

    //     Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    //     Gizmos.DrawWireSphere(transform.position + Vector3.up * 0, 0.2f);
    // }
    // test
    // private void OnDrawGizmos()
    // {
    //     EditorGizmo(transform);
    //     Gizmos.color = UnityEngine.Color.red;
    //     Gizmos.DrawWireSphere(transform.position, info.attackRange);
    // }
}