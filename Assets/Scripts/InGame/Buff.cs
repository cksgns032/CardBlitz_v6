using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    float buffTime;
    BuffData buffData;
    // ���� ����
    int hp;// ��
    int defence;// ����
    float moveSpeed;// �̵��ӵ�
    public float attackSpeed;// ���ݼӵ�
    float attackRange;// ���ݹ���
    int attackCnt;// ���ݰ��� ��
    float attack;// ���ݷ�
    float criPercent;// ũ��Ƽ�� Ȯ��
    float criAdd;// ũ��Ƽ�� ������ ����
    float currentTime;

    //WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSecondsRealtime waitReal = new WaitForSecondsRealtime(0.1f);
    Monster player;

    public void SetBuff(BuffData data, Monster player)
    {
        buffData = data;
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
