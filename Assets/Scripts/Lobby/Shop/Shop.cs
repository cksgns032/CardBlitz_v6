using System;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    LobbyType state = LobbyType.SHOP;

    Button exitBtn;
    LobbyScene lobbyScene;

    // Start is called before the first frame update
    public void Init()
    {
        lobbyScene = GetComponentInParent<LobbyScene>();
        exitBtn = gameObject.transform.Find("ExitBtn").GetComponent<Button>();
        exitBtn.onClick.AddListener(ExitClick);
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
