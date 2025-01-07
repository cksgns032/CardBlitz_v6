using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    float buffTime;
    // 버프 내용
    int hp;// 피
    int defence;// 방어력
    float moveSpeed;// 이동속도
    float attackSpeed;// 공격속도
    float attackRange;// 공격범위
    int attackCnt;// 공격가능 수
    float attack;// 공격력
    float criPercent;// 크리티컬 확률
    float criAdd;// 크리티컬 데미지 곱셈
    float currentTime;

    WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSecondsRealtime waitReal = new WaitForSecondsRealtime(0.1f);
    Player player;

    public void SetBuff(BuffData data, Player player)
    {
        this.player = player;
    }
    public void StartActivation()
    {
        StartCoroutine(Activation());
    }
    IEnumerator Activation()
    {
        while (currentTime > 0)
        {
            currentTime -= 0.1f;
            yield return waitReal;
        }
        currentTime = 0;
        DeActivation();
    }
    

    // Update is called once per frame
    void DeActivation()
    {
        if(player && !player.GetState())
        {
            player.ReMoveBuff(this);
        }
    }
}
