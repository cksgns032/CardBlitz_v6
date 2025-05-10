public struct SendData
{
    public ObserveData observeData;
    public string data;
    public void SetData(ObserveData observeData, string data)
    {
        this.observeData = observeData;
        this.data = data;
    }
}
public enum ObserveData
{
    ChangeMain,
}

#region lobby
public enum LobbyType
{
    HOME,
    CARD,
    SHOP,
}
#endregion

#region common
public enum PopUp_Name
{
    Option,
    Fade,
    Count,
}
public enum PopUp_State
{
    Open,
    Close,
}
#endregion

#region game
public enum RESULT
{
    WIN,
    LOSE,
    DRAW,
}
public enum Team
{
    Red = 0,
    Blue = 1,
    None = 2,
}
public enum StateType
{
    Idle,
    Move,
    Charge,
    Attack,
    Hit,
    Die,
}
public enum AttackType
{
    None,
    Ground,
    Midair,
    All,
}
public enum EventButtonType
{
    NONE,
    TOP,// ����ӵ� ����(�Ʊ�)
    MIDDLE,// �̼� ���(�Ʊ�)
    BOTTOM,// ���� ����(��)
}
#endregion