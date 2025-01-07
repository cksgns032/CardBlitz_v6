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

    Team enemyColor;

    public void Init()
    {
        // 결과 애니메이션
        resultTxt = gameObject.transform.Find("ResultText").GetComponent<Text>();
        resultTxt.gameObject.SetActive(false);
        resultAni = resultTxt.gameObject.GetComponent<Animation>();

        // 내 정보
        myProfile = gameObject.transform.Find("MyProfile").GetComponentInChildren<GameProfile>();
        myProfile.Init();
        myProfile.GetColor(UserData.team);

        // 적 팀 세팅
        enemyColor = UserData.team == Team.Red ? Team.Blue : Team.Red;
        // 적 정보
        enemyProfile = gameObject.transform.Find("EnemyProfile").GetComponentInChildren<GameProfile>();
        if (cardGroup) 
        { 
            enemyProfile.Init(); 
            enemyProfile.GetColor(enemyColor); 
        }

        cardGroup = gameObject.GetComponentInChildren<CardGroup>(true);
        if (cardGroup) { 
            cardGroup.Init(); 
        }

        shuffleBtn = gameObject.transform.Find("Shuffle").GetComponent<Button>();
        shuffleBtn.onClick.AddListener(cardGroup.Shuffle);

        Timer = gameObject.GetComponentInChildren<GameTimer>(true);
        if (Timer) { 
            Timer.Init(); 
        }
    }
    #region 게이지 관리
    public void UseCost(Team team, int useCost)
    {
        if(team == UserData.team)
        {
            myProfile.UpdateGauge(useCost);
        }
        else
        {
            enemyProfile.UpdateGauge(useCost);
        }
    }
    #endregion 게이지 관리
    #region 타임 관리
    public void UpdateTime(float timeNum)
    {
        Timer.UpdateTimer(timeNum);
    }
    #endregion 타임 관리

    // 결과 나타냄
    public void Result(RESULT result)
    {
        resultTxt.gameObject.SetActive(true);
        resultTxt.color = new Color(255, 97, 97, 255);
        GameManager.Instance.SetClear(true);
        switch (result)
        {
            case RESULT.WIN:
                resultTxt.text = "WIN!!";
                resultAni.Play("ResultWin");
                break;
            case RESULT.LOSE:
                resultTxt.text = "Lose..";
                resultAni.Play("ResultLose");
                break;
            case RESULT.DRAW:
                resultTxt.text = "Draw";
                resultAni.Play("ResultDraw");
                break;
        }
        Invoke("LobbyGo", 5);
    }
    public void UpdateTower(Team hitTeam, float attack)
    {
        if(hitTeam == UserData.team)
        {
            myProfile.SetTowerHp(attack);
        }
        else
        {
            enemyProfile.SetTowerHp(attack);
        }
    }
    
    public GameProfile MyProfile()
    {
        return myProfile;
    }
    public GameProfile EnemyProfile()
    {
        return enemyProfile;
    }
    public void CargeGauge()
    {
        if(myProfile)
        {
            myProfile.UpdateGauge(1);
        }
        if(enemyProfile)
        {
            enemyProfile.UpdateGauge(1);
        }
    }
    public void CargeCard()
    {
        cardGroup.AddCard();
    }
}
