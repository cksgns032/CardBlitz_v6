using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    SphereCollider sphereCollider;
    Monster playerCom;
    PlayerState state;

    public void Init()
    {
        sphereCollider = GetComponent<SphereCollider>();
        playerCom = GetComponentInParent<Monster>();
        state = playerCom.GetState();
    }
    public void SetRadius(float radius)
    {
        if (sphereCollider)
        {
            sphereCollider.radius = radius;
        }

    }
    // 들어왔을 때 리스트에 넣어줌
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear())
            return;

        LayerMask enemyLayer = LayerMask.NameToLayer("ENEMY");
        if (other.gameObject.layer == enemyLayer)
        {
            if (other.gameObject.tag == "Player")
            {
                Monster player = other.GetComponent<Monster>();
                if (player && player.IsDie() == false)
                {
                    playerCom.AddEemyList(player);
                }
            }
            else if (other.gameObject.tag == "EnemyTower")
            {
                Tower player = other.GetComponent<Tower>();
                playerCom.AddEemyList(player);
            }
        }
    }
}
