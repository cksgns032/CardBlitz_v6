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


public enum PacketType : ushort
{
    None = 0,
    PlayerMove = 1,
    Chat = 2,
    // ... 기타 패킷 타입
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
public enum Layer_Type
{
    UI,
    Popup,
    Count
}
public enum UI_Name
{
    FadeUI,
    Count,
}
public enum PopUp_Name
{
    OptionPopup,
    ResultPopup,
    Count,
}
public enum Active_State
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
    TOP,
    MIDDLE,
    BOTTOM,
}
#endregion