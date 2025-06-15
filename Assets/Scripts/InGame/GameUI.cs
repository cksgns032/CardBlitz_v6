using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    GameProfile myProfile;
    GameProfile enemyProfile;
    Text resultTxt;
    Animation resultAni;
    CardGroup cardGroup;
    Button shuffleBtn;
    GameTimer Timer;
    Coroutine cardChargeCoroutine;

    public void Init()
    {
        // my data
        myProfile = gameObject.transform.Find("MyProfile").GetComponentInChildren<GameProfile>();
        if (myProfile)
        {
            myProfile.Init();
            myProfile.GetColor(GameManager.Instance.GetMyGameData().team);
        }

        // enemy data
        enemyProfile = gameObject.transform.Find("EnemyProfile").GetComponentInChildren<GameProfile>();
        if (enemyProfile)
        {
            enemyProfile.Init();
            enemyProfile.GetColor(GameManager.Instance.GetMyGameData().team == TeamType.Red ? TeamType.Blue : TeamType.Red);
        }

        shuffleBtn = gameObject.transform.Find("Shuffle").GetComponent<Button>();

        // card
        cardGroup = gameObject.GetComponentInChildren<CardGroup>(true);
        if (cardGroup)
        {
            cardGroup.Init();
        }

        shuffleBtn.onClick.AddListener(cardGroup.Shuffle);

        // timer
        Timer = gameObject.GetComponentInChildren<GameTimer>(true);
        if (Timer)
        {
            Timer.Init();
        }

        // charge
        GaugeFill();
        CardFill();
    }
    public void GaugeFill()
    {
        StartCoroutine(GaugeFill(GameManager.Instance.GetGaugeTime()));
    }
    IEnumerator GaugeFill(float gaugeTime)
    {
        while (GameManager.Instance.GetClear() == false)
        {
            yield return new WaitForSeconds(gaugeTime);
            ChargeGauge();
        }
    }
    public void ChargeGauge()
    {
        if (myProfile)
        {
            myProfile.UpdateGauge(1);
        }
        if (enemyProfile)
        {
            enemyProfile.UpdateGauge(1);
        }
    }
    public void CardFill()
    {
        cardChargeCoroutine = StartCoroutine(IECardFill());
    }
    public void StopCardFill()
    {
        if (cardChargeCoroutine != null)
        {
            StopCoroutine(cardChargeCoroutine);
            cardChargeCoroutine = null;
        }
    }
    IEnumerator IECardFill()
    {
        float elapsedTime = 0f;
        float cargeTime = GameManager.Instance.GetCardTime();

        while (elapsedTime < cargeTime && GameManager.Instance.GetClear() == false)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= cargeTime)
            {
                elapsedTime = 0f;
                ChargeCard();
            }
            yield return null;
        }
    }
    public void ChargeCard()
    {
        cardGroup.AddCard();
    }
    #region Cost 
    public void UseCost(TeamType team, int useCost)
    {
        if (team == GameManager.Instance.GetMyGameData().team)
        {
            myProfile.UpdateGauge(useCost);
        }
        else
        {
            enemyProfile.UpdateGauge(useCost);
        }
    }
    #endregion Cost
    #region Time
    public void UpdateTime(float timeNum)
    {
        Timer.UpdateTimer(timeNum);
    }
    #endregion Time

    #region result
    public void Result(ResultType result)
    {
        resultTxt.gameObject.SetActive(true);
        resultTxt.color = new Color(255, 97, 97, 255);
        GameManager.Instance.SetClear(true);
        switch (result)
        {
            case ResultType.WIN:
                resultTxt.text = "WIN!!";
                resultAni.Play("ResultWin");
                break;
            case ResultType.LOSE:
                resultTxt.text = "Lose..";
                resultAni.Play("ResultLose");
                break;
            case ResultType.DRAW:
                resultTxt.text = "Draw";
                resultAni.Play("ResultDraw");
                break;
        }
        Invoke("LobbyGo", 5);
    }
    #endregion result
    #region Tower
    public void UpdateTower(TeamType hitTeam, float attack)
    {
        if (hitTeam == GameManager.Instance.GetMyGameData().team)
        {
            myProfile.SetTowerHp(attack);
        }
        else
        {
            enemyProfile.SetTowerHp(attack);
        }
    }
    #endregion Tower
    public Button GetShuffle()
    {
        return shuffleBtn;
    }
}
