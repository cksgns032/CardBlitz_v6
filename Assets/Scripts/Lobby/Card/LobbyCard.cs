using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCard : MonoBehaviour
{
    Button exitBtn;
    GetCard getCard;
    SelectHero heroInfo;
    Lobby lobby;
    HeroInfo info;

    HeroCard[] cards;

    // Start is called before the first frame update
    public void Init()
    {
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitClick);
        lobby = GetComponentInParent<Lobby>();
        info = gameObject.transform.Find("HeroInfo").GetComponent<HeroInfo>();
        info.Init();
        info.gameObject.SetActive(false);
        cards = GameObject.FindObjectsByType<HeroCard>(FindObjectsSortMode.None);//GameObject.FindObjectsOfType<HeroCard>();
        foreach (var card in cards)
        {
            if (card != null) { card.Init(); }
        }
    }
    public void Setting()
    {

    }
    public void ExitClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
    }
}
