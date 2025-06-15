using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : SingleTon<PoolingManager>
{
    int defaultCapacity = 10;
    int maxPoolSize = 15;
    string objName;

    public IObjectPool<GameObject> DamageTxtPool { get; private set; }
    public Dictionary<ushort, IObjectPool<GameObject>> MonsterPoolList = new Dictionary<ushort, IObjectPool<GameObject>>();

    public void Init()
    {
        DamageTxtPool = new ObjectPool<GameObject>(CreateDamageTxtPoolItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
        for (int i = 0; i < defaultCapacity; i++)
        {
            DamageTxt txt = CreateDamageTxtPoolItem().GetComponent<DamageTxt>();
            txt.GetComponent<DamageTxt>().Pool = this.DamageTxtPool;
            txt.Pool.Release(txt.gameObject);
        }
    }

    public void SetPool(ushort unitID)
    {
        // objName
        if (unitID >= 0)
        {
            ObjectPool<GameObject> monsterPool = new ObjectPool<GameObject>(CreateMonsterPoolItem, OnTakeFromPool, OnReturnedToPool,
            OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
            for (int i = 0; i < defaultCapacity; i++)
            {
                Monster monster = CreateMonsterPoolItem().GetComponent<Monster>();
                monster.Pool = monsterPool;
                monster.Pool.Release(monster.gameObject);
            }
            MonsterPoolList.Add(unitID, monsterPool);
        }
    }

    private GameObject CreateDamageTxtPoolItem()
    {
        Transform parant = FindFirstObjectByType<GameUI>().transform.Find("DamageTextGroup");
        GameObject poolGo = Instantiate(Resources.Load<GameObject>("Prefabs/InGame/DamageTxt"), parant); poolGo.GetComponent<DamageTxt>().Pool = this.DamageTxtPool;
        return poolGo;
    }

    private GameObject CreateMonsterPoolItem()
    {
        Transform parant = GameObject.Find("MonsterGroup").transform;
        GameObject poolGo = Instantiate(Resources.Load<GameObject>("Prefabs/Monster/" + objName), parant);
        if (poolGo != null)
        {
            return poolGo;
        }
        else
        {
            return null;
        }
    }
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}
