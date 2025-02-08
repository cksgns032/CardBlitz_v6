using UnityEngine;
using UnityEngine.AI;

public class IdleState : ButtonSearch, IState
{
    NavMeshAgent agent;
    public void Init(Player data)
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
    }
    public void Exit()
    {

    }

    public void StateUpdate()
    {

    }
}
