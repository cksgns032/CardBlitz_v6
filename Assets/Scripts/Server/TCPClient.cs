using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.Antlr3.Runtime;

public class TCPClient : SingleTon<TCPClient>
{
    Socket client;
    int clientID;
    string nickName;
    bool connect = false;
    bool isConnect = false;

    public bool Connect(string ipAddress, int port, string nickName)
    {
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(ipAddress, port);
        this.nickName = nickName;
        connect = true;

        return connect;
    }
    private void OnApplicationQuit()
    {
        if(client != null)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }

    public void SendMsg(string msg)
    {
        if (msg.Equals(string.Empty) == false)
        {
            byte[] sendmsg = System.Text.Encoding.UTF8.GetBytes(msg);
            client.Send(sendmsg);
        }
    }
    // Start is called before the first frame update
    public void Init()
    {
        isConnect = true;
        Connect("127.0.0.1", 80,"Kim Chan Hun");
    }

    // Update is called once per frame
    void Update()
    {
        if (isConnect == false)
            return;
        UpdateClient();
    }
    public void UpdateClient()
    {
        if(client != null && client.Poll(0,SelectMode.SelectRead))
        {
            byte[] buffer = new byte[1024];
            int recvLen = client.Receive(buffer);

            // 첫번째 데이터가 모드형식
            string packetStr = System.Text.Encoding.UTF8.GetString(buffer);

            packetStr = packetStr.Replace("\0", "");
            string[] arr = packetStr.Split(",");

            GameProtocolType type;

            if (System.Enum.TryParse(arr[0], out type) == false) return;
            Debug.Log(type);
            switch (type)
            {
                case GameProtocolType.Init:
                    int.TryParse(arr[1], out clientID);
                    UserData.uniqueID = clientID;
                    packetStr = string.Format($"{GameProtocolType.USERINFO},{clientID},{nickName}");
                    byte[] sendPack = System.Text.Encoding.UTF8.GetBytes(packetStr);
                    client.Send(sendPack);
                    break;
                case GameProtocolType.GoGame:
                    UserData.team = (Team)int.Parse(arr[1]);
                    Debug.Log(UserData.team);
                    StartCoroutine(IENextScene());
                    Debug.Log("GameScene Go");
                    break;
                case GameProtocolType.START:
                    break;
                case GameProtocolType.CHAT:
                    break;
                case GameProtocolType.END:
                    RESULT resultType;
                    if(System.Enum.TryParse(arr[1], out resultType) == false) return;
                    GameManager.Instance.ResultGame(resultType);
                    break;
                case GameProtocolType.USERINFO:
                    break;
                case GameProtocolType.CLIENTOBJ:
                    Team team;
                    if (System.Enum.TryParse(arr[3], out team) == false) return;
                    if (team == UserData.team)
                        return;
                    // arr[1] = 위치
                    // arr[2] = 오브젝트 이름 
                    Player player = Instantiate(Resources.Load<Player>("Prefabs/" + arr[2]));
                    player.gameObject.layer = LayerMask.NameToLayer("ENEMY");
                    //player.Init();
                    player.AgentMaskSet(arr[1],team);
                    break;
            }
        }
    }
    public void CreateObj(string player, string pos)
    {
        if (isConnect == false)
            return;
        string pack = string.Format($"{GameProtocolType.CREATEOBJ},{pos},{player},{UserData.team}");
        byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(pack);
        client.Send(sendArr);
    }
    public void EndGame(RESULT result)
    {
        string pack = string.Format($"{GameProtocolType.END},{result}");
        byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(pack);
        client.Send(sendArr);
    }
    public void SendPack<T>(GameProtocolType type, T content)
    {
        if (isConnect == false)
            return;
        string pack = string.Format($"{type},{content}");
        byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(pack);
        client.Send(sendArr);
    }
    IEnumerator IENextScene()
    {
        //yield return new WaitForSeconds(1);

        //fade.FadeOut();

        yield return new WaitForSeconds(1);

        AsyncOperation asyn = SceneManager.LoadSceneAsync("GameScene");
        //gameObject.SetActive(false);
        while (!asyn.isDone)
        {
            //asyn.allowSceneActivation = false;
            yield return null;
        }

    }
}
