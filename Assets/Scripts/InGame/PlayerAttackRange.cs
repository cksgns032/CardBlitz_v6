using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    SphereCollider sphereCollider;
    public void Init()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear())
            return;
        Debug.Log("Attack Range : ",other);
    }
}
