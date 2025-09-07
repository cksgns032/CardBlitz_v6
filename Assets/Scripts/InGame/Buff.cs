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
        Debug.Log("remove buff");
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            if (buffList[i].buffName == buff.buffName)
            {
                Debug.Log($"remove : {buffList[i]}");
                buffList.RemoveAt(i);
                break;
            }
        }
        player.SetStat();
    }
    public void AddBuff(BuffData data)
    {
        Debug.Log($"add buff : {data}");
        buffList.Add(data);
        player.SetStat();
    }
    public int BuffHp()
    {
        List<BuffData> BuffList = buffList.Where(x => x.hpInt > 0 || x.hpPercent > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            if (buff.hpInt > 0)
            {
                buffValue += buff.hpInt;
            }
            if (buff.hpPercent > 0)
            {
                buffValue += player.GetHeroData().hp * buff.hpPercent;
            }
        }
        return (int)Math.Round(buffValue);
    }
    public int BuffDefence()
    {
        List<BuffData> BuffList = buffList.Where(x => x.defenceInt > 0 || x.defencePercent > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            if (buff.defenceInt > 0)
            {
                buffValue += buff.defenceInt;
            }
            if (buff.defencePercent > 0)
            {
                buffValue += player.GetHeroData().defence * buff.defencePercent;
            }
        }
        return (int)Math.Round(buffValue);
    }
    public float BuffMoveSpeed()
    {
        List<BuffData> BuffList = buffList.Where(x => x.moveSpeed > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            buffValue += player.GetHeroData().moveSpeed * buff.moveSpeed;
        }
        return buffValue;
    }
    public float BuffAttackSpeed()
    {
        List<BuffData> BuffList = buffList.Where(x => x.attackSpeed > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            buffValue += player.GetHeroData().attackSpeed * buff.attackSpeed;
        }
        return buffValue;
    }
    public int BuffAttackCnt()
    {
        List<BuffData> BuffList = buffList.Where(x => x.attackCnt > 0).ToList();
        int buffValue = 0;
        foreach (var buff in BuffList)
        {
            buffValue += buff.attackCnt;
        }
        return buffValue;
    }
    public float BuffAttackRange()
    {
        List<BuffData> BuffList = buffList.Where(x => x.attackRange > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            buffValue += player.GetHeroData().attackRange * buff.attackRange;
        }
        return buffValue;
    }
    public float BuffDamage()
    {
        List<BuffData> BuffList = buffList.Where(x => x.attackInt > 0 || x.attackPercent > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            if (buff.attackInt > 0)
            {
                buffValue += buff.attackInt;
            }
            if (buff.attackPercent > 0)
            {
                buffValue += player.GetHeroData().attack * buff.attackPercent;
            }
        }
        return buffValue;
    }
    public float BuffCri()
    {
        List<BuffData> BuffList = buffList.Where(x => x.criInt > 0 || x.criPercent > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            if (buff.criInt > 0)
            {
                buffValue += buff.criInt;
            }
            if (buff.criPercent > 0)
            {
                buffValue += player.GetHeroData().criPercent * buff.criPercent;
            }
        }
        return buffValue;
    }
    public float BuffCriDamage()
    {
        List<BuffData> BuffList = buffList.Where(x => x.criDamagePercent > 0).ToList();
        float buffValue = 0;
        foreach (var buff in BuffList)
        {
            if (buff.criDamagePercent > 0)
            {
                buffValue += player.GetHeroData().criDamage * buff.criDamagePercent;
            }
        }
        return buffValue;
    }
}
