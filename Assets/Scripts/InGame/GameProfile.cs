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

    // Start is called before the first frame update
    public void Init()
    {
        // todo : hard coding delete
        hp.value = 1;// hard code
        gauge.maxValue = 5;// hard code
        gauge.value = UserData.gauge;// hard code
        gaugeNum.text = gauge.value.ToString();
    }
    public void GetColor(Team team)
    {
        if (team == Team.Red)
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
            GameManager.Instance.ResultGame(RESULT.LOSE);
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
        UserData.gauge = gauge.value;
    }
}
