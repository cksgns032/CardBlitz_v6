using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 네트워크 라이브러리
using System.Net;
// 통신 클래스
using System.Net.Sockets;
using System;

public enum GameProtocolType
{
    Init = 100, // 초기화 세팅
    Ready,      // 게임 레디
    GoGame,     // 게임씬으로 이동
    START,      // 게임 시작
    CHAT,       // 채팅
    END,        // 게임 종료
    USERINFO,   // 유저 정보
    CREATEOBJ,  // 서버 몬스터 생성 데이터 받음
    CLIENTOBJ,  // 클라 몬스터 생성 
}

public class TCPServer : MonoBehaviour
{
    // 클라이언트의 접속 요청시 클라이언트의 복사본 소켓 생성
    Socket server;
    // 클라이언트 리스트
    List<Socket> clients = new List<Socket>();

    //------------
    Dictionary<int, string> userInfo = new Dictionary<int, string>();
    int uniqueID = 0;
    int readyPlayerNum = 0;

    // 접속을 처리하는 함수를 연결하기 위한 델리게이트
    System.Action accept = null;

    void CreateServer()
    {
        server = new Socket(AddressFamily.InterNetwork,// 네자리수 체계를 사용하는 ipc주소
                            SocketType.Stream,
                            ProtocolType.Tcp);

        // EndPoint : (ip주소 + 포트) 목적지
        server.Bind( new IPEndPoint(IPAddress.Any, 80));

        server.Listen(2);// 최대 접속자 수

        accept += Accept;

    }
    void Accept()
    {
         // 접속 요청 소켓 유무
         if(server.Poll(0,SelectMode.SelectRead))
        {
            ++uniqueID;

            Socket client = server.Accept();

            // 서버가 클라이언트에게 대기 패킷을 보내면
            // 클라이언트는 서버에게 자신의 정보값을 다시 보냄

            string packet = string.Format($"{GameProtocolType.Init},{uniqueID},{client}");
            byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(packet);

            client.Send(sendArr);

            clients.Add(client);
            // 접속이 다 되었다면 Accept()함수가 실행되지 않게 처리
            if(clients.Count > 1)
            {
                accept = null;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (server == null)
            return;

        accept?.Invoke();

        UpdateClient();

        // 동기화 함수(통신은 보통 비동기 함수 사용)
        // 서버소켓으로부터 읽을 데이터가 있으면 처리
        if (server.Poll(0, SelectMode.SelectRead))// Poll(대기시간, 읽기 or 쓰기) : 누군가가 접속 요청하는지
        {
            Socket client = server.Accept();
            clients.Add(client);
        }

        // c# 함수 (전송, 수신) 들은 byte[]배열값으로 처리
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[1024];
                // 읽어들인 사이즈 값을 받습니다
                int recvLength = clients[i].Receive(buffer);

                // 받은 데이터가 있으면 다른 클라이언트한테 보낸다
                if (recvLength == 0)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < clients.Count; j++)
                        clients[j].Send(buffer);
                }
            }
        }
    }
    public void UpdateClient()
    {
        for(int i = 0; i < clients.Count; i++)
        {
            if(clients[i].Poll(0,SelectMode.SelectRead))
            {

                byte[] buffer = new byte[1024];

                int recvLen = clients[i].Receive(buffer);

                if(recvLen > 0)
                {
                    string packStr = System.Text.Encoding.UTF8.GetString(buffer);

                    packStr.Replace("\0", "");

                    string[] packArr = packStr.Split(",");

                    GameProtocolType type;
                    if(System.Enum.TryParse(packArr[0], out type) != false)
                    {
                        switch (type)
                        {
                            case GameProtocolType.Init:
                                break;
                            case GameProtocolType.Ready:
                                readyPlayerNum++;
                                //packArr[1]
                                if(readyPlayerNum >= 2)
                                {
                                    System.Random rand = new System.Random();
                                    int value = rand.Next(0,2);
                                    for (int k = 0; k < clients.Count; k++)
                                    {
                                        string pack = string.Empty;
                                        if(k == 0)
                                        {
                                            pack = string.Format($"{GameProtocolType.GoGame},{value}");
                                        }
                                        else if(k == 1)
                                        {
                                            if (value == 0)
                                                value = 1;
                                            else if(value == 1)
                                                value = 0;

                                            pack = string.Format($"{GameProtocolType.GoGame},{value}");
                                        }
                                        byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(pack);
                                        clients[k].Send(sendArr);
                                    }
                                    readyPlayerNum = 0;
                                }
                                break;
                            case GameProtocolType.START:
                                break;
                            case GameProtocolType.END:
                                for (int j = 0; j < clients.Count; j++)
                                {
                                    string pack = string.Format($"{GameProtocolType.END},{packArr[1]},Capsule,{packArr[3]}");
                                    byte[] sendarr = System.Text.Encoding.UTF8.GetBytes(pack);
                                    clients[j].Send(sendarr);
                                    Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));
                                }
                                break;
                            case GameProtocolType.USERINFO:
                                int userID = 0;
                                if(int.TryParse(packArr[1],out userID))
                                {
                                    userInfo.Add(userID, packArr[2]);
                                }
                                Debug.Log("Get Info");
                                break;
                            case GameProtocolType.CHAT:
                                break;
                            case GameProtocolType.CREATEOBJ:
                                for (int j = 0; j < clients.Count; j++)
                                {
                                    string pack = string.Format($"{GameProtocolType.CLIENTOBJ},{packArr[1]},DarkNight,{packArr[3]}");
                                    byte[] sendarr = System.Text.Encoding.UTF8.GetBytes(pack);
                                    clients[j].Send(sendarr);
                                    Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

    public void SendUserInfo()
    {
        string user = string.Empty;

    }

    private void OnApplicationQuit()
    {
        for(int i = 0; i < clients.Count; i++)
        {
            clients[i].Shutdown(SocketShutdown.Both);
            clients[i].Close();
        }
        if (server != null)
            server.Close(); 
    }
}
