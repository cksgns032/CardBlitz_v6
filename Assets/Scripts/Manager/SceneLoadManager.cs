using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingleTon<SceneLoadManager>
{
    Fade fade;
    bool isLoading = false;
    public void LoadSceneMode(string sceneName)
    {
        if (!isLoading)
        {
            fade = (Fade)UIManager.Instance.GetPopUp(PopUp_Name.Fade, true);
            isLoading = true;
            StartCoroutine(IENextScene(sceneName));
        }
    }
    IEnumerator IENextScene(string sceneName)
    {
        if (fade)
        {
            fade.FadeOut();
        }

        yield return new WaitForSeconds(1);

        AsyncOperation asyn = SceneManager.LoadSceneAsync(sceneName);
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
