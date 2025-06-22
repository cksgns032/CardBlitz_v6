public enum DataTable
{
    MissionData,
    UnitData,
}
public enum ObserveData
{
    ChangeMain,
}
public enum PacketType : ushort
{
    None = 0,
    UnitPos = 1,
    CreateUnit = 2,
    Chat,

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
public enum ResultType
{
    WIN,
    LOSE,
    DRAW,
}
public enum TeamType
{
    None = 0,
    Red = 1,
    Blue = 2,
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
    None = 0,
    Ground,
    Midair,// 공중
    All,
}
public enum LineType
{
    NONE = 0,
    TOP = 1,
    MIDDLE = 2,
    BOTTOM = 3,
    COUNT
}
#endregion