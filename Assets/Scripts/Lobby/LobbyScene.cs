using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    Home home;
    CardSetting cardSetting;
    Shop shop;
    Observer observer;

    public void Init()
    {
        home = gameObject.GetComponentInChildren<Home>();
        home.Init();
        cardSetting = gameObject.GetComponentInChildren<CardSetting>();
        cardSetting.Init();
        shop = gameObject.GetComponentInChildren<Shop>();
        shop.Init();

        Home();
    }
    public void ChangeScene(string state)
    {
        if (home) home.gameObject.SetActive(home.GetState() == state);
        if (cardSetting) cardSetting.gameObject.SetActive(cardSetting.GetState() == state);
        if (shop) shop.gameObject.SetActive(shop.GetState() == state);
        UIManager.Instance.AllClose();
        SendData sendData = new SendData();
        sendData.SetData(ObserveData.ChangeMain, state);
        ObserverManager.Instance.Notify(sendData);
    }
    public void GameStart()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");

        //UIManager.Instance.OpenPopUp(PopUp_Name.Loading);

        SceneLoadManager.Instance.LoadSceneMode("GameScene");

        //TCPClient.Instance.SendPack(GameProtocolType.Ready,UserData.uniqueID);
    }
    public void Home()
    {
        ChangeScene(Enum.GetName(typeof(LobbyType), LobbyType.HOME));
    }

    public void Notify()
    {

    }
}
