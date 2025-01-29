using UnityEngine;
using UnityEngine.AI;

public class DieState : MonoBehaviour, IState
{
    Player player;
    NavMeshAgent agent;
    PlayerState stateCom;
    Animator ani;
    public DieState(Player data)
    {
        player = data;
        stateCom = GetComponent<PlayerState>();
        ani = GetComponentInChildren<Animator>();
    }
    public void Enter()
    {
        Die();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
    public void Die()
    {
        if (agent == null)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        player.SetDie(true);
        if (stateCom.GetOldType() == StateType.Charge)
        {
            EventButton btn = player.GetEventButton();
            if (btn != null)
            {
                btn.Charging(false);
            }
            player.SetEventButton(null);
        }
        ani.SetTrigger("Death");
    }
}
