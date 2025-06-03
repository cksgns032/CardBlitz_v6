using System.Collections.Generic;

public struct HeroData
{
    public int hp;
    public int defence;
    public float moveSpeed;
    public Team team;
    public AttackType attackType;
    public float attackSpeed;
    public float attackRange;
    public int attackCnt;
    public float attack;
    public float criPercent;
    public float criAdd;
}
public struct BuffData
{
    public float buffTime;
    public int hpInt;
    public float hpPercent;
    public int defenceInt;
    public float defencePercent;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public int attackCnt;
    public float attackInt;
    public float attackPercent;
    public float criInt;
    public float criPercent;
    public float criDamageInt;
    public float criDamagePercent;
}
public struct UnitPositionData
{
    public short UnitId; // 2 bytes
    public short PosX;   // 2 bytes (양자화된 값)
    public short PosY;   // 2 bytes (양자화된 값)
}