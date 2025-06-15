using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    Image img;
    //MeshRenderer mesh;
    bool charging = false;
    TeamType getColor = TeamType.None;
    public void Init()
    {
        BuffData data = new BuffData();
        data.buffTime = 999;
        switch (gameObject.tag)
        {
            case "TOP":
                {
                    // 공격력 증가
                    data.attackPercent = 1.5f;
                }
                break;
            case "MIDDLE":
                {
                    // 방어력 증가
                    data.defencePercent = 1.5f;
                }
                break;
            case "BOTTOM":
                {
                    // 이속 증가가
                    data.moveSpeed = 2f;
                }
                break;
        }

        img = gameObject.GetComponentInChildren<Canvas>(true).GetComponentInChildren<Image>(true);
        if (img)
        {
            img.enabled = true;
            img.fillAmount = 0;
        }
    }
    public bool ChargeImage(float num, string layer, TeamType team)
    {
        img.enabled = true;
        img.fillAmount += num;
        if (img.fillAmount == 1)
        {
            // 점령을 해서 버프를 주기 위해
            for (int i = 0; i < GameManager.Instance.GetMyList().Count; i++)
            {
                GameManager.Instance.GetMyList()[i].SetStat();
            }
            for (int i = 0; i < GameManager.Instance.GetEnemyList().Count; i++)
            {
                GameManager.Instance.GetEnemyList()[i].SetStat();
            }
            // ����ġ �� ������ ����
            switch (layer)
            {
                case "ENEMY":
                    getColor = team == TeamType.Blue ? TeamType.Red : TeamType.Blue;
                    break;
                case "HERO":
                    getColor = team == TeamType.Blue ? TeamType.Blue : TeamType.Red;
                    break;
            }
            charging = false;
            img.fillAmount = 0;
            img.enabled = false;
            return true;
        }
        return false;
    }
    public void Charging(bool state)
    {
        charging = state;
        if (state == false)
        {
            img.fillAmount = 0;
        }
    }
    public bool CheckState()
    {
        return charging;
    }
    public TeamType GetColor()
    {
        return getColor;
    }
    public void SetColor(TeamType team)
    {
        getColor = team;
    }
}
