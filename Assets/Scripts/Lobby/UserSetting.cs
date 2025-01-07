using System;
using UnityEngine.UI;

public class UserSetting : UIBase, Observer
{
    public Text nickname;
    public Text level;
    public Text gold;
    public Text gem;
    private void OnEnable()
    {
        ObserverManager.Instance.AddObserver(this);
    }
    private void OnDisable()
    {
        ObserverManager.Instance.RemoveObserver(this);
    }
    public void SetInfo()
    {

    }

    public void Notify(SendData data)
    {
        switch (data.observeData)
        {
            case ObserveData.ChangeMain:
                {
                    if (data.data == Enum.GetName(typeof(LobbyType), LobbyType.HOME))
                    {
                        transform.Find("Profile").gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.Find("Profile").gameObject.SetActive(false);
                    }
                }
                break;
        }
    }
}
