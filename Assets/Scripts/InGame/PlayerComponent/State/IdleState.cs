using UnityEngine.AI;

public class IdleState : ButtonSearch, IState
{
    NavMeshAgent agent;
    public void Init(Monster data)
    {
        player = data;
        stateCom = data.GetState();
        agent = player.gameObject.GetComponent<NavMeshAgent>();
    }
    public void Enter()
    {
        if (agent)
        {
            agent.isStopped = true;
        }
        if (player)
        {
            player.AttackCoolTime();
        }
    }
    public void Exit()
    {

    }
    public void StateUpdate()
    {
        if (player.IsDie() == true || GameManager.Instance.GetClear())
        {
            return;
        }
    }
}
