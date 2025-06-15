public interface IState
{
    public void Init(Monster player);
    public void Enter();
    public void Exit();
    public void StateUpdate();
}

public interface Observer
{
    public void Notify(byte[] buffer);
}