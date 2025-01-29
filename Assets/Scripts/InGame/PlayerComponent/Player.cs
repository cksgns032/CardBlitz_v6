using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //CapsuleCollider colder;
    NavMeshAgent agent;
    Rigidbody rigid;
    Animator ani;
    PlayerState stateCom;
    [SerializeField] PlayerAttackRange attackRangeCom;
    EventButton btnEvent;
    [SerializeField] HeroData info = new HeroData();
    [SerializeField] List<Buff> buffList = new List<Buff>();
    [SerializeField] List<Player> enemyList = new List<Player>();
    bool isDie = false;

    public void Init()
    {
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
        stateCom = GetComponent<PlayerState>();
        if (stateCom != null)
        {
            stateCom.Init(this);
        }
    }
    public List<Player> GetEnemyList()
    {
        return enemyList;
    }
    public void SetStat()
    {
        info.hp = 100;
        info.defence = 1;
        info.attack = 100;
        info.attackSpeed = 5;
        //waitAttack = new WaitForSeconds(info.attackSpeed);
        info.attackCnt = 1;
        info.attackRange = 10;
        info.moveSpeed = 1f;
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
            stateCom.TransState(StateType.Move);
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
        stateCom.TransState(StateType.Hit);
        stateCom.Damage(damage);
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
    }
    public void AddBuff(BuffData data)
    {
        Buff buff = new Buff();
        buff.SetBuff(data, this);
        buffList.Add(buff);
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
        return isDie;
    }
    public void Destroy()
    {
        Destroy(gameObject);
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