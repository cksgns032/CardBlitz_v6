public interface IState
{
    public void Init(Player player);
    public void Enter();
    public void Exit();
    public void StateUpdate();
}