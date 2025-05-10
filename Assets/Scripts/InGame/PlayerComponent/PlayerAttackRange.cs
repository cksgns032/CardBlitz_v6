using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    SphereCollider sphereCollider;
    Player playerCom;
    PlayerState state;

    public void Init()
    {
        sphereCollider = GetComponent<SphereCollider>();
        playerCom = GetComponentInParent<Player>();
        state = playerCom.GetState();
    }
    // 들어왔을 때 리스트에 넣어줌
    private void OnTriggerEnter(Collider other)
    {
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
