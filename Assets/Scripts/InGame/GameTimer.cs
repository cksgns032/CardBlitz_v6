using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    Text timeText;
    public void Init()
    {
        timeText = GetComponentInChildren<Text>();
    }

    public void UpdateTimer(float timeNum)
    {
        if (timeNum <= 0)
        {
            return;
        }
        //num -= Time.deltaTime;
        string str1 = string.Format("{0:00}", (int)(timeNum / 60 % 60));
        string str2 = string.Format("{0:00}", (int)(timeNum % 60));
        timeText.text = str1 + ":" + str2;
        if (timeNum == 60)
        {
            GameManager.Instance.LastBuffe();
        }
        
    }
}
