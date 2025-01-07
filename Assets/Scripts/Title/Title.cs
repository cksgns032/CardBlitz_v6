using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : SceneBaseManager
{
    Animation tap;
    Animation titleIdle;
    Fade fade;

    // Start is called before the first frame update
    void Start()
    {
        base.SetScale();
        //Screen.SetResolution(1280, 720, FullScreenMode.Windowed);

        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "TitleSound");
        AudioManager.Instance.PlayBgm(true, "TitleSound");
        AudioManager.Instance.LoadSound(AudioManager.Type.SFX, "Click");

        UIManager.Instance.Init();
        UIManager.Instance.LoadPrefabs();

        //TCPClient.Instance.Init();
        tap = GetComponentInChildren<Animation>(true);
        tap.Play();

        titleIdle = GetComponent<Animation>();
        titleIdle.Play();

        fade = (Fade)UIManager.Instance.GetPopUp(PopUp_Name.Fade,false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
            StartCoroutine(IENextScene());
        }
    }
    IEnumerator IENextScene()
    {
        yield return new WaitForSeconds(1);
        if(fade)
        {
            fade.FadeOut();
        }
        yield return new WaitForSeconds(1);

        AsyncOperation asyn = SceneManager.LoadSceneAsync("LobbyScene");
        while (!asyn.isDone)
        {
            yield return null;
        }
    }
}
