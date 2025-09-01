using UnityEngine;

public abstract class SceneUIManager : MonoBehaviour
{
    public GameObject popUpLayer;
    public GameObject uiLayer;

    public abstract void Init();
}
