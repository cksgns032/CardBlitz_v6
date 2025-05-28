using UnityEngine;
using UnityEngine.AI;

public class ChargeState : MonoBehaviour, IState
{
    Monster player;
    NavMeshAgent agent;
    PlayerState stateCom;
    UserGameData userData;
    public void Init(Monster data)
    {
        userData = GameManager.Instance.GetMyGameData();
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
        EventButton btn = player.GetEventButton();
        if (btn != null)
        {
            player.SetEventButton(null);
        }
    }

    public void StateUpdate()
    {
        if (player.IsDie() == true || GameManager.Instance.GetClear())
        {
            return;
        }

        EventButton btn = player.GetEventButton();
        if (btn != null &&
            btn.GetColor() != userData.team)
        {
            if (btn.ChargeImage(1 * Time.deltaTime, LayerMask.LayerToName(gameObject.layer), userData.team))
            {
                player.SetEventButton(btn);
                stateCom.TransState(StateType.Move);
            }
        }
    }
}
