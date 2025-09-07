using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    Image img;
    //MeshRenderer mesh;
    bool charging = false;
    TeamType getColor = TeamType.None;
    private BuffData buffData = new BuffData();
    public void Init()
    {
        buffData.buffTime = 999;
        buffData.buffEndTime = 10000;
        switch (gameObject.tag)
        {
            case "TOP":
                {
                    buffData.buffName = BuffName.TopBuff;
                    // 공격력 증가
                    buffData.attackPercent = 1.5f;
                }
                break;
            case "MIDDLE":
                {
                    buffData.buffName = BuffName.MiddleBuff;
                    // 방어력 증가
                    buffData.defencePercent = 1.5f;
                }
                break;
            case "BOTTOM":
                {
                    buffData.buffName = BuffName.BottomBuff;
                    // 이속 증가가
                    buffData.moveSpeed = 2f;
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
            // todo : 팀과 비교 후 내 팀이면 추가 해주고
            // 아니면 빼 준다
            List<Monster> myList = GameSceneManager.Instance.GetSceneManager<GameSceneManager>().GetMyList();
            List<Monster> enemyList = GameSceneManager.Instance.GetSceneManager<GameSceneManager>().GetEnemyList();
            for (int i = 0; i < myList.Count; i++)
            {
                myList[i].AddBuff(buffData);
                myList[i].SetStat();
            }
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].ReMoveBuff(buffData);
                enemyList[i].SetStat();
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
            Charging(false);
            // todo : 서버 연결 후 완료 보내기
            return true;
        }
        return false;
    }
    public void Charging(bool state)
    {
        charging = state;
        img.enabled = state;
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
