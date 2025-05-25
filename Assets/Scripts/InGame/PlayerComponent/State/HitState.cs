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

    public void Init(Player data)
    {
        player = data;
        stateCom = data.GetState();
        ani = player.gameObject.GetComponentInChildren<Animator>();
        agent = player.gameObject.GetComponent<NavMeshAgent>();
        if (damageObj == null)
        {
            damageObj = transform.Find("DamagePos").gameObject;
        }
    }
    public void Enter()
    {
        stat = player.GetStat();
    }

    public void Exit()
    {

    }

    public void StateUpdate()
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
        stat.hp -= (int)damage;
        Debug.Log($"hp : {stat.hp}");
        if (stat.hp > 0)
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
