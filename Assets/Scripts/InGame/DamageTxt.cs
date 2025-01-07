using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamageTxt : MonoBehaviour
{
    Text txt;
    public IObjectPool<GameObject> Pool { get; set; }
    // Start is called before the first frame update
    public void Setting(Vector3 pos, float damage)
    {
        transform.position = Camera.main.WorldToScreenPoint(pos);
        txt = gameObject.GetComponentInChildren<Text>();
        txt.text = damage.ToString();
        Invoke("Relese", 0.5f);
    }
    void Relese()
    {
        Pool.Release(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
