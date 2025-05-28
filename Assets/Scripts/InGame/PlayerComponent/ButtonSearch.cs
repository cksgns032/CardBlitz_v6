using UnityEngine;

public class ButtonSearch : MonoBehaviour
{
    protected Monster player;
    protected PlayerState stateCom;
    UserGameData userData;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear() && player.IsDie())
        {
            return;
        }
        int layerNum = LayerMask.NameToLayer("EVENTBUTTON");
        if (other.gameObject.layer == layerNum)
        {
            EventButton btn = other.gameObject.GetComponent<EventButton>();
            if (btn && btn.CheckState() == false && btn.GetColor() != userData.team)
            {
                player.SetEventButton(btn);
                btn.Charging(true);
                stateCom.TransState(StateType.Charge);
            }
        }
    }
    // private void OnTriggerStay(Collider other)
    // {
    //     if (GameManager.Instance.GetClear() && player.IsDie())
    //     {
    //         return;
    //     }
    //     // 유닛 공격
    //     if (player.GetEnemyList().Count > 0 && stateCom.GetCurrentType() != StateType.Charge)
    //     {
    //         stateCom.TransState(StateType.Attack);
    //     }
    // }
}
