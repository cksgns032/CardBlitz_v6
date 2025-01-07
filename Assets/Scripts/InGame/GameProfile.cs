using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameProfile : MonoBehaviour
{
    [SerializeField] Image thumbnail;
    [SerializeField] Text nick;
    [SerializeField] Slider hp;
    [SerializeField] Slider gauge;
    [SerializeField] Text gaugeNum;

    // Start is called before the first frame update
    public void Init()
    {
        thumbnail = GetComponentInChildren<Image>();
        nick = GetComponentInChildren<Text>();
        nick.text = "nick";
        hp = gameObject.transform.Find("Hp").GetComponent<Slider>();
        hp.value = 1;
        gauge = gameObject.transform.Find("Gauge").GetComponent<Slider>();
        gauge.maxValue = 5;
        gauge.value = UserData.gauge = 0;
        gaugeNum = gauge.transform.Find("GaugeNume").GetComponent<Text>();
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
