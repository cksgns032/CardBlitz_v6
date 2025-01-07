using UnityEngine;
using UnityEngine.UI;

public class SceneBaseManager : MonoBehaviour
{
    GameObject popUpLayer;
    public virtual void SetScale()
    {
        int setWidth = 1920;
        int setHeight = 1080;
        float fixedAspect = setWidth / setHeight;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;
        float current = Screen.width / Screen.height;
        CanvasScaler canvas;
        canvas = gameObject.GetComponent<CanvasScaler>();

        if (current > fixedAspect)
            canvas.matchWidthOrHeight = 1;
        else
        {
            canvas.matchWidthOrHeight = 0;
        }
        popUpLayer = GameObject.Find("PopUpLayer");
    }
    public GameObject GetPopUpLayer()
    {
        return popUpLayer;
    }
}
