using UnityEngine;
using UnityEngine.AI;

public class ChargeState : MonoBehaviour, IState
{
    Player player;
    NavMeshAgent agent;
    PlayerState stateCom;
    public ChargeState(Player data)
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
        EventButton btn = player.GetEventButton();
        if (btn != null)
        {
            player.SetEventButton(null);
        }
    }

    public void Update()
    {
        if (player.IsDie() == true || GameManager.Instance.GetClear())
        {
            return;
        }

        EventButton btn = player.GetEventButton();
        if (btn != null &&
            btn.GetColor() != UserData.team)
        {
            if (btn.ChargeImage(1 * Time.deltaTime, LayerMask.LayerToName(gameObject.layer)))
            {
                player.SetEventButton(null);
                stateCom.TransState(StateType.Move);
            }
        }
    }
}
