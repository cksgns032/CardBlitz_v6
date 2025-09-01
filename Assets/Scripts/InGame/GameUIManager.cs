using UnityEngine;

public class GameUIManager : SceneUIManager
{
    public GameHud gameHud;
    public override void Init()
    {
        gameHud = GetComponentInChildren<GameHud>(true);
        gameHud?.Init();
    }
}
