using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : SceneBaseManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "TitleSound");
        AudioManager.Instance.PlayBgm(true, "TitleSound");
        AudioManager.Instance.LoadSound(AudioManager.Type.SFX, "Click");

        UIManager.Instance.LoadPrefabs();
        SetScale();
        //TCPClient.Instance.Init();
    }
}
