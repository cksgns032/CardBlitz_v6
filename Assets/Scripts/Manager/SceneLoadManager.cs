using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingleTon<SceneLoadManager>
{
    FadeUI fade;
    bool isLoading = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void LoadSceneMode(SceneName sceneName)
    {
        if (!isLoading)
        {
            fade = (FadeUI)UIManager.Instance.GetUI(UI_Name.FadeUI);
            isLoading = true;
            StartCoroutine(IENextScene(sceneName));
        }
    }
    IEnumerator IENextScene(SceneName sceneName)
    {
        if (fade)
        {
            fade.FadeOut();
        }

        yield return new WaitForSeconds(1);

        string name = Enum.GetName(typeof(SceneName), sceneName);
        AsyncOperation asyn = SceneManager.LoadSceneAsync(name);
        while (!asyn.isDone)
        {
            yield return null;
        }
        if (asyn.isDone)
        {
            isLoading = false;
        }
    }
}
