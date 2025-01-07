using System.IO;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class Mission
{
    public int ID;
    public string TYPE;
    public string NAME;
    public string STORY;
    public int CURRENT;
    public int GOAL;
    public string REWARDTYPE;
    public int REWARD;
    public bool ISCLEAR;
}

public class Lobby : SceneBaseManager, Observer
{
    LobbyScene lobbyScene;
    UserSetting userInfo;
    Fade fade;

    Mission[] missionList;
    private void OnEnable()
    {
        base.SetScale();
        ObserverManager.Instance.AddObserver(this);
    }
    private void OnDisable()
    {
        ObserverManager.Instance.RemoveObserver(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Init();
        UIManager.Instance.AllClose();

        // Áö¿ï °Í 
        UIManager.Instance.LoadPrefabs();

        AudioManager.Instance.LoadSound(AudioManager.Type.BGM, "TitleSound");
        AudioManager.Instance.PlayBgm(true, "TitleSound");
        AudioManager.Instance.LoadSound(AudioManager.Type.SFX, "Click");

        lobbyScene = gameObject.GetComponentInChildren<LobbyScene>();
        lobbyScene.Init();

        userInfo = gameObject.GetComponentInChildren<UserSetting>();
        userInfo.SetInfo();

        LoadJson();

        fade = (Fade)UIManager.Instance.GetPopUp(PopUp_Name.Fade, true);
        fade.FadeIn();
    }

    void LoadJson()
    {
        /*string json = File.ReadAllText(Application.dataPath + "/Resources/GameData/MissionData.json");
        JsonData jsondata = JsonUtility..ToObject(json);
        missionList = new Mission[jsondata.Count];
        for(int i = 0; i < jsondata.Count; i++)
        {
            Mission mission = new Mission();
            mission.ID = int.Parse(jsondata[i]["ID"].ToString());
            mission.TYPE = jsondata[i]["TYPE"].ToString();
            mission.NAME = jsondata[i]["NAME"].ToString();
            mission.STORY = jsondata[i]["STORY"].ToString();
            mission.CURRENT = int.Parse(jsondata[i]["CURRENT"].ToString());
            mission.GOAL = int.Parse(jsondata[i]["GOAL"].ToString());
            mission.REWARDTYPE = jsondata[i]["REWARDTYPE"].ToString();
            mission.REWARD = int.Parse(jsondata[i]["REWARD"].ToString());
            mission.ISCLEAR = jsondata[i]["ISCLEAR"].ToString() == "TRUE" ? true : false;
            missionList[i] = mission;
        }*/
    }

    public void Notify(SendData data)
    {
    }
}
