using UnityEngine;

public class TitleUIManager : SceneUIManager
{
    Animation tap;
    Animation titleIdle;

    void Start()
    {
        tap = GetComponentInChildren<Animation>(true);
        tap.Play();

        titleIdle = GetComponent<Animation>();
        titleIdle.Play();
        TitleSceneManager cc = TitleSceneManager.Instance as TitleSceneManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Click, "Click");
            SceneLoadManager.Instance.LoadSceneMode(SceneName.GameScene);
        }
    }
}
