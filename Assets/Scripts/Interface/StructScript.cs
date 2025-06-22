using System;

public struct UnitData
{
    public string name;
    public int hp;
    public int defence;
    public float attack;
    public float attackSpeed;
    public int attackCnt;
    public float attackRange;
    public AttackType attackType;
    public float moveSpeed;
    public float criPercent;
    public float criAdd;

    public void Setting(string[] data)
    {
        name = data[1];
        hp = int.Parse(data[2]);
        defence = int.Parse(data[3]);
        attack = int.Parse(data[4]);
        attackSpeed = int.Parse(data[5]);
        attackCnt = int.Parse(data[6]);
        attackRange = int.Parse(data[7]);
        System.Enum.TryParse<AttackType>(Enum.GetName(typeof(AttackType), int.Parse(data[8])), out attackType);
        moveSpeed = int.Parse(data[9]);
        criPercent = int.Parse(data[10]);
        criAdd = 0;
    }
}
public struct HeroData
{
    public int hp;
    public int defence;
    public float moveSpeed;
    public TeamType team;
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