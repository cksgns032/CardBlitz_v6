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
        idleState = new IdleState(player);
        moveState = new MoveState(player);
        attackState = new AttackState(player);
        hitState = new HitState(player);
        chargeState = new ChargeState(player);
        dieState = new DieState(player);
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
    }
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
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
    }
}
