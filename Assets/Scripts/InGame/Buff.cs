using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] List<BuffData> buffList = new List<BuffData>();

    //WaitForSeconds wait = new WaitForSeconds(0.1f);
    WaitForSecondsRealtime waitReal = new WaitForSecondsRealtime(0.1f);
    Monster player;

    public void Init()
    {
        player = GetComponent<Monster>();
    }
    void Update()
    {
        if (buffList.Count > 0)
        {
            float deltaTime = Time.deltaTime;
            for (int i = buffList.Count - 1; i >= 0; i--)
            {
                if (buffList[i].update(deltaTime))
                {
                    buffList.RemoveAt(i);
                    break;
                }
            }
        }
    }
    public void ReMoveBuff(BuffData buff)
    {
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            if (buffList[i].buffName == buff.buffName)
            {
                buffList.RemoveAt(i);
                break;
            }
        }
        Debug.Log(buffList);
    }
    public void AddBuff(BuffData data)
    {

        buffList.Add(data);
        foreach (var a in buffList)
        {
            Debug.Log($"{a}");
        }
    }

    public float BuffAttackCoolTime()
    {
        List<BuffData> attackSpeedBuffList = buffList.Where(x => x.attackSpeed > 0).ToList();
        float attackCoolTimebuff = 0;
        foreach (var buff in attackSpeedBuffList)
        {
            attackCoolTimebuff += player.GetHeroData().attackSpeed * buff.attackSpeed;
        }
        return attackCoolTimebuff;
    }
}
