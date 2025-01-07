using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;

public class PlayerData
{
    public string name;
    public int score;
}
public class WebSocketManager : SingleTon<WebSocketManager>
{
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://localhost:3000");
        //서버에서 설정한 포트를 넣어줍니다.
        ws.OnMessage += Ws_Message;
        ws.Connect();
        ws.Send("Hellow");
    }
    void Ws_Message(object sender, MessageEventArgs e)
    {
        Debug.Log("adress : "+ ((WebSocket)sender).Url+ "data : "+ e.Data);
    }
    // post : 추가한다
    public void PostData()
    {
        StartCoroutine(IEPostData());
    }
    IEnumerator IEPostData()
    {
        PlayerData data = new PlayerData { name = "Player", score = 100 };
        string json = JsonUtility.ToJson(data);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/game-data", json, "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error : " + request.error);
            }
            else
            {
                Debug.Log("server Respones : " + request.downloadHandler.text);
            }
        }
    }
    IEnumerator IEGetData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:3000/login"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error : " + request.error);
            }
            else
            {
                Debug.Log("server Respones : " + request.downloadHandler.text);
            }
        }
    }
}
