using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameProfile : MonoBehaviour
{
    public Image thumbnail;
    public Text nick;
    public Slider hp;
    public Slider gauge;
    public Text gaugeNum;
    UserGameData userData;

    // Start is called before the first frame update
    public void Init()
    {
        userData = GameSceneManager.Instance.GetSceneManager<GameSceneManager>().GetMyGameData();
        // todo : hard coding delete
        hp.value = 1;// hard code
        gauge.maxValue = 5;// hard code
        gauge.value = userData.gauge;// hard code
        gaugeNum.text = gauge.value.ToString();
    }
    // todo : 서버 생기면 서버 연결
    public void GetColor(TeamType team)
    {
        if (team == TeamType.Red)
            thumbnail.color = Color.green;
        else
            thumbnail.color = Color.blue;
    }
    public void SetTowerHp(float attack)
    {
        float num = hp.value - attack;
        if (num <= 0)
        {
            num = 0;
            GameSceneManager.Instance.GetSceneManager<GameSceneManager>().ResultGame(ResultType.LOSE);
        }

        hp.value = num;
    }
    public Slider GetGauge()
    {
        return gauge;
    }
    public void UpdateGauge(int cost)
    {
        if (gauge.value >= 5)
        {
            return;
        }
        gauge.value += cost;
        gaugeNum.text = gauge.value.ToString();
        userData.gauge = gauge.value;
    }
}
