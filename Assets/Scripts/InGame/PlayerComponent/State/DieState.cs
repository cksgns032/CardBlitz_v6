using UnityEngine;
using UnityEngine.AI;

public class DieState : MonoBehaviour, IState
{
    Monster player;
    NavMeshAgent agent;
    PlayerState stateCom;
    Animator ani;
    public void Init(Monster data)
    {
        player = data;
        stateCom = data.GetState();
        ani = gameObject.GetComponent<Animator>();
    }
    public void Enter()
    {
        Die();
    }

    public void Exit()
    {

    }

    public void StateUpdate()
    {

    }
    public void Die()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
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
        ani.SetTrigger("Die");
    }
    public void EndDieEvent()
    {
        player.gameObject.SetActive(false);
    }
}
