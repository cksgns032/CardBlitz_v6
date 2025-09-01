using UnityEngine;
using UnityEngine.UI;

public class SceneBaseManager : SingleTon<SceneBaseManager>
{
    protected SceneUIManager sceneUI;
    virtual public void Start()
    {
        sceneUI = GameObject.FindAnyObjectByType<SceneUIManager>();
    }
    public virtual void SetScale()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        float fixedAspect = setWidth / setHeight;

        float current = Screen.width / Screen.height;
        CanvasScaler canvas;
        canvas = gameObject.GetComponent<CanvasScaler>();

        if (canvas != null)
        {
            if (current > fixedAspect)
            {
                canvas.matchWidthOrHeight = 1;
            }
            else
            {
                canvas.matchWidthOrHeight = 0;
            }
        }

    }
    public T GetUIManager<T>() where T : SceneUIManager
    {
        Debug.Log($"{typeof(T).Name}");
        return sceneUI as T;
    }
    public SceneUIManager GetSceneUI()
    {
        return sceneUI;
    }
    public T GetSceneManager<T>() where T : SceneBaseManager
    {
        Debug.Log($"{typeof(T).Name}");
        return Instance as T;
    }
}
