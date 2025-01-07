using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ��Ʈ��ũ ���̺귯��
using System.Net;
// ��� Ŭ����
using System.Net.Sockets;
using System;

public enum GameProtocolType
{
    Init = 100, // �ʱ�ȭ ����
    Ready,      // ���� ����
    GoGame,     // ���Ӿ����� �̵�
    START,      // ���� ����
    CHAT,       // ä��
    END,        // ���� ����
    USERINFO,   // ���� ����
    CREATEOBJ,  // ���� ���� ���� ������ ����
    CLIENTOBJ,  // Ŭ�� ���� ���� 
}

public class TCPServer : MonoBehaviour
{
    // Ŭ���̾�Ʈ�� ���� ��û�� Ŭ���̾�Ʈ�� ���纻 ���� ����
    Socket server;
    // Ŭ���̾�Ʈ ����Ʈ
    List<Socket> clients = new List<Socket>();

    //------------
    Dictionary<int, string> userInfo = new Dictionary<int, string>();
    int uniqueID = 0;
    int readyPlayerNum = 0;

    // ������ ó���ϴ� �Լ��� �����ϱ� ���� ��������Ʈ
    System.Action accept = null;

    void CreateServer()
    {
        server = new Socket(AddressFamily.InterNetwork,// ���ڸ��� ü�踦 ����ϴ� ipc�ּ�
                            SocketType.Stream,
                            ProtocolType.Tcp);

        // EndPoint : (ip�ּ� + ��Ʈ) ������
        server.Bind( new IPEndPoint(IPAddress.Any, 80));

        server.Listen(2);// �ִ� ������ ��

        accept += Accept;

    }
    void Accept()
    {
         // ���� ��û ���� ����
         if(server.Poll(0,SelectMode.SelectRead))
        {
            ++uniqueID;

            Socket client = server.Accept();

            // ������ Ŭ���̾�Ʈ���� ��� ��Ŷ�� ������
            // Ŭ���̾�Ʈ�� �������� �ڽ��� �������� �ٽ� ����

            string packet = string.Format($"{GameProtocolType.Init},{uniqueID},{client}");
            byte[] sendArr = System.Text.Encoding.UTF8.GetBytes(packet);

            client.Send(sendArr);

            clients.Add(client);
            // ������ �� �Ǿ��ٸ� Accept()�Լ��� ������� �ʰ� ó��
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

        // ����ȭ �Լ�(����� ���� �񵿱� �Լ� ���)
        // �����������κ��� ���� �����Ͱ� ������ ó��
        if (server.Poll(0, SelectMode.SelectRead))// Poll(���ð�, �б� or ����) : �������� ���� ��û�ϴ���
        {
            Socket client = server.Accept();
            clients.Add(client);
        }

        // c# �Լ� (����, ����) ���� byte[]�迭������ ó��
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].Poll(0, SelectMode.SelectRead))
            {
                byte[] buffer = new byte[1024];
                // �о���� ������ ���� �޽��ϴ�
                int recvLength = clients[i].Receive(buffer);

                // ���� �����Ͱ� ������ �ٸ� Ŭ���̾�Ʈ���� ������
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
