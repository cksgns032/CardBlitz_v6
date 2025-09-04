using UnityEngine;

public class GameUIManager : SceneUIManager
{
    public HudComponent gameHud;
    public override void Init()
    {
        gameHud = GetComponentInChildren<HudComponent>(true);
        gameHud?.Init();
    }
}
