using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
[Union((int)PacketType.CreateUnit, typeof(CreateUnitPacket))]
[Union((int)PacketType.UnitPos, typeof(UnitPosPacket))]
public class Packet
{
    [Key(0)]
    public ushort packID { get; set; }
}
[MessagePackObject]
public class UnitPosPacket : Packet
{
    [Key(1)]
    public virtual Dictionary<short, short[]> posList { get; set; }
}
[MessagePackObject]
public class UseMana : Packet
{
    // 몇 마나 소모, 소모 타입(마법, 유닛 소환 등...), uid
    [Key(1)]
    public virtual uint playerUid { get; set; }
    [Key(2)]
    public virtual ushort useMana { get; set; }
    [Key(3)]
    public virtual ushort UseType { get; set; }
}
[MessagePackObject]
public class CreateUnitPacket : Packet
{
    // 팀, 코스트, 라인, 유닛 
    [Key(1)]
    public virtual ushort lineID { get; set; }
    [Key(2)]
    public virtual ushort unitID { get; set; }
    [Key(3)]
    public virtual ushort unitGrade { get; set; }
    [Key(4)]
    public virtual ushort useTeam { get; set; }

    public void SetData(ushort packId, ushort line, ushort unitId, ushort grade, ushort team)
    {
        packID = packId;
        lineID = line;
        unitID = unitId;
        unitGrade = grade;
        useTeam = team;
    }
}