using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    Animator ani;
    SphereCollider sphereCollider;
    Player playerCom;

    public void Init()
    {
        ani = GetComponentInParent<Animator>(true);
        sphereCollider = GetComponent<SphereCollider>();
        playerCom = GetComponentInParent<Player>();
    }
    // 들어왔을 때 리스트에 넣어줌
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ontrigger enter");
        if (GameManager.Instance.GetClear())
            return;
        Player player = other.GetComponent<Player>();
        LayerMask enemyLayer = LayerMask.NameToLayer("ENEMY");
        if (player && other.gameObject.tag == "Player" && other.gameObject.layer == enemyLayer)
        {
            playerCom.AddEemyList(player);
        }
    }
}
