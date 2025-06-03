using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System;

public class Home : MonoBehaviour
{
    LobbyType state = LobbyType.HOME;

    ButtonExtension cardBtn;
    ButtonExtension shopBtn;
    ButtonExtension gameStart;
    ButtonExtension option;

    GameObject btnGroup;
    LobbyScene lobbyScene;
    public void Init()
    {
        // ����Ʈ ����
        lobbyScene = GetComponentInParent<LobbyScene>();
        btnGroup = gameObject.transform.Find("BtnGroup").gameObject;

        cardBtn = btnGroup.transform.Find("Card").GetComponent<ButtonExtension>();

        cardBtn.AddListener(() => lobbyScene.ChangeScene(Enum.GetName(typeof(LobbyType), LobbyType.CARD)));

        shopBtn = btnGroup.transform.Find("Shop").GetComponent<ButtonExtension>();
        shopBtn.AddListener(() => lobbyScene.ChangeScene(Enum.GetName(typeof(LobbyType), LobbyType.SHOP)));

        gameStart = gameObject.transform.Find("GameStart").GetComponent<ButtonExtension>();
        gameStart.AddListener(lobbyScene.GameStart);

        option = gameObject.transform.Find("Setting").GetComponent<ButtonExtension>();
        option.AddListener(Option);
    }
    public string GetState()
    {
        return Enum.GetName(typeof(LobbyType), state);
    }
    public void SetState(LobbyType _state)
    {
        state = _state;
    }
    public void Option()
    {
        UIBase option = UIManager.Instance.GetPopUp(PopUp_Name.OptionPopup);
        option.Draw();
    }
}
