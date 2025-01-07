using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolingManager : SingleTon<PoolingManager>
{
    int defaultCapacity = 10;
    int maxPoolSize = 15;
    GameObject obj;
    public IObjectPool<GameObject> Pool { get; private set; }
    
    public void Init()
    {
        Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
        OnDestroyPoolObject, true, defaultCapacity, maxPoolSize);
        for (int i = 0; i < defaultCapacity; i++)
        {
            DamageTxt txt = CreatePooledItem().GetComponent<DamageTxt>();
            txt.Pool.Release(txt.gameObject);
        }
    }
    // 생성
    private GameObject CreatePooledItem()
    {
        Transform parant = FindFirstObjectByType<GameUI>().transform.Find("DamageTextGroup");
        GameObject poolGo = Instantiate(Resources.Load<GameObject>("Prefabs/DamageTxt"), parant);
        poolGo.GetComponent<DamageTxt>().Pool = this.Pool;
        return poolGo;
    }

    // 사용
    private void OnTakeFromPool(GameObject poolGo)
    {
        poolGo.SetActive(true);
    }

    // 반환
    private void OnReturnedToPool(GameObject poolGo)
    {
        poolGo.SetActive(false);
    }

    // 삭제
    private void OnDestroyPoolObject(GameObject poolGo)
    {
        Destroy(poolGo);
    }
}
