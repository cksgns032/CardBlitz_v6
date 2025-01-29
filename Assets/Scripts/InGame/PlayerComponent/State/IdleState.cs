using UnityEngine;
using UnityEngine.AI;

public class IdleState : ButtonSearch, IState
{
    NavMeshAgent agent;
    public IdleState(Player data)
    {
        player = data;
        stateCom = GetComponent<PlayerState>();
        agent = GetComponent<NavMeshAgent>();
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

    public void Update()
    {

    }
}
