using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetting : MonoBehaviour
{
    LobbyType state = LobbyType.CARD;

    Button exitBtn;
    LobbyScene lobbyScene;

    // Start is called before the first frame update
    public void Init()
    {
        lobbyScene = GetComponentInParent<LobbyScene>();
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitClick);
    }
    public void Setting()
    {
        // 유저가 가지고 있는 카드 세팅
        // 유저가 사용하고 있는 카드 세팅
    }
    public string GetState()
    {
        return Enum.GetName(typeof(LobbyType), state);
    }
    public void SetState(LobbyType _state)
    {
        state = _state;
    }
    public void ExitClick()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
        lobbyScene.Home();
    }
}
