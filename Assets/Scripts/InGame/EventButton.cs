using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    TOP,// ����ӵ� ����(�Ʊ�)
    MIDDLE,// �̼� ���(�Ʊ�)
    BOTTOM,// ���� ����(��)
}
public class EventButton : MonoBehaviour
{
    Image img;
    MeshRenderer mesh;

    bool charging = false;
    Team getColor = Team.None;
    public Type posType;
    // Start is called before the first frame update
    public void Init()
    {
        mesh = gameObject.transform.Find("Button 01").GetComponent<MeshRenderer>();
        img = gameObject.GetComponentInChildren<Canvas>(true).GetComponentInChildren<Image>(true);
        img.enabled = true;
        img.fillAmount = 0;
    }
    public bool ChargeImage(float num, string team)
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
            switch (team)
            {
                case "ENEMY":
                    if (UserData.team == Team.Blue)
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/RedButton");
                        getColor = Team.Red;
                    }
                    else
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/BlueButton");
                        getColor = Team.Blue;

                    }
                    break;
                case "HERO":
                    if (UserData.team == Team.Blue)
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/BlueButton");
                        getColor = Team.Blue;
                    }
                    else
                    {
                        mesh.material = Resources.Load<Material>("Prefabs/Material/RedButton");
                        getColor = Team.Red;
                    }
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
    public Team GetColor()
    {
        return getColor;
    }
    public void SetColor(Team team)
    {
        getColor = team;
    }
}
