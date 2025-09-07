using UnityEngine;

public class ButtonSearch : MonoBehaviour
{
    protected Monster player;
    protected PlayerState stateCom;
    private void OnTriggerEnter(Collider other)
    {
        if (GameSceneManager.Instance.GetSceneManager<GameSceneManager>().GetClear() && player.IsDie())
        {
            return;
        }
        int layerNum = LayerMask.NameToLayer("EVENTBUTTON");
        if (other.gameObject.layer == layerNum)
        {
            EventButton btn = other.gameObject.GetComponent<EventButton>();
            // todo : 서버 연결 후 주석 조건문에 넣어주기
            if (btn && btn.CheckState() == false)//&& btn.GetColor() != GameSceneManager.Instance.GetSceneManager<GameSceneManager>().GetMyGameData().team
            {
                player.SetEventButton(btn);
                btn.Charging(true);
                stateCom.TransState(StateType.Charge);
            }
            else
            {
                Debug.Log("btn skip");
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
