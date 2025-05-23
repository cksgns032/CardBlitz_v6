using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    private IState oldState;
    private IState currentState;
    private StateType currentType;
    private StateType oldType;
    private IdleState idleState;
    private MoveState moveState;
    private AttackState attackState;
    private HitState hitState;
    private ChargeState chargeState;
    private DieState dieState;
    private Player player;

    public void Init(Player playerCom)
    {
        player = playerCom;
        idleState = player.AddComponent<IdleState>();
        if (idleState)
        {
            idleState.Init(player);
        }
        moveState = player.AddComponent<MoveState>();
        if (moveState)
        {
            moveState.Init(player);
        }
        attackState = player.AddComponent<AttackState>();
        if (attackState)
        {
            attackState.Init(player);
        }
        hitState = player.AddComponent<HitState>();
        if (hitState)
        {
            hitState.Init(player);
        }
        chargeState = player.AddComponent<ChargeState>();
        if (chargeState)
        {
            chargeState.Init(player);
        }
        dieState = player.AddComponent<DieState>();
        if (dieState)
        {
            dieState.Init(player);
        }
    }
    public void TransState(StateType state)
    {
        if (oldState != currentState)
        {
            oldType = currentType;
            oldState = currentState;
        }
        if (oldState != null)
        {
            oldState.Exit();
        }
        currentState = GetCurrentState(state);
        currentState.Enter();
        Debug.Log(currentState);
    }
    public void Update()
    {
        if (currentState != null)
        {
            currentState.StateUpdate();
        }
    }
    public IState GetCurrentState(StateType state)
    {
        currentType = state;
        switch (state)
        {
            case StateType.Idle:
                {
                    return idleState;
                }
            case StateType.Move:
                {
                    return moveState;
                }
            case StateType.Attack:
                {
                    return attackState;
                }
            case StateType.Hit:
                {
                    return hitState;
                }
            case StateType.Die:
                {
                    return dieState;
                }
            case StateType.Charge:
                {
                    return chargeState;
                }
        }
        return null;
    }
    public StateType GetCurrentType()
    {
        return currentType;
    }
    public StateType GetOldType()
    {
        return oldType;
    }
    public void Damage(float damage)
    {
        hitState.Damage(damage);
        TransState(StateType.Hit);
    }
}
