using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    float buffTime;
    // ���� ����
    int hp;// ��
    int defence;// ����
    float moveSpeed;// �̵��ӵ�
    float attackSpeed;// ���ݼӵ�
    float attackRange;// ���ݹ���
    int attackCnt;// ���ݰ��� ��
    float attack;// ���ݷ�
    float criPercent;// ũ��Ƽ�� Ȯ��
    float criAdd;// ũ��Ƽ�� ������ ����
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
    void DeActivation()
    {
        if (player && !player.IsDie())
        {
            player.ReMoveBuff(this);
        }
    }
}
