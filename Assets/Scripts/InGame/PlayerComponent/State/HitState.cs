using UnityEngine;
using UnityEngine.AI;

public class HitState : MonoBehaviour, IState
{
    GameObject damageObj;
    Player player;
    HeroData stat;
    Animator ani;
    NavMeshAgent agent;
    PlayerState stateCom;

    public HitState(Player data)
    {
        player = data;
        stateCom = GetComponent<PlayerState>();
        ani = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public void Enter()
    {
        if (damageObj == null)
        {
            damageObj = transform.Find("DamagePos").gameObject;
        }
        stat = player.GetStat();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
    public void Damage(float damage)
    {
        GameObject obj = PoolingManager.Instance.Pool.Get();
        DamageTxt txt = obj.GetComponent<DamageTxt>();
        if (txt != null)
        {
            txt.Setting(damageObj.transform.position, damage);
        }
        if (stat.hp - damage > 0)
        {
            stat.hp -= (int)damage;
            ani.SetTrigger("Hit");
        }
        else
        {
            stateCom.TransState(StateType.Die);
        }
    }
}
