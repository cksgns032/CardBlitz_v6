#region 로비
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
public enum LobbyType
{
    HOME,
    CARD,
    SHOP,
}
public enum PopUp_Type
{
    Popup,
    UserInfo,
    Count,
}
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
#region 인게임 
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
public enum AttackType
{
    None,
    Ground,
    Midair,
    All,
}
public enum MosterState
{
    Move,
    Charge,
    Attack,
    Hit,
    Die,
}
#endregion