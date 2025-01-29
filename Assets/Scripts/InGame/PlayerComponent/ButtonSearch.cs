using UnityEngine;

public class ButtonSearch : MonoBehaviour
{
    protected Player player;
    protected PlayerState stateCom;
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetClear() && player.IsDie())
        {
            return;
        }
        if (other.gameObject.tag == "EVENTBUTTON")
        {
            EventButton btn = other.gameObject.GetComponent<EventButton>();
            if (btn && btn.CheckState() == false && btn.GetColor() != UserData.team)
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
