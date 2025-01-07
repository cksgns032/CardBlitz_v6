public struct HeroData
{
    public int hp;// 피
    public int defence;// 방어력
    public float moveSpeed;// 이동속도
    public Team team;// 팀
    public AttackType attackType;// 공격 가능 타입
    public float attackSpeed;// 공격속도
    public float attackRange;// 공격범위
    public int attackCnt;// 공격가능 수
    public float attack;// 공격력
    public float criPercent;// 크리티컬 확률
    public float criAdd;// 크리티컬 데미지 곱셈
}
public struct BuffData
{
    // 지속시간
    public float buffTime;
    // 버프 내용
    public int hp;// 피
    public int defence;// 방어력
    public float moveSpeed;// 이동속도
    public float attackSpeed;// 공격속도
    public float attackRange;// 공격범위
    public int attackCnt;// 공격가능 수
    public float attack;// 공격력
    public float criPercent;// 크리티컬 확률
    public float criAdd;// 크리티컬 데미지 곱셈
}
