using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class TestTCP : SingleTon<TestTCP>
{
    public string serverAddress = "127.0.0.1"; // 접속할 서버 IP 주소
    public int serverPort = 8888;             // 접속할 서버 포트

    private TcpClient _client;
    private NetworkStream _stream;
    private Thread _receiveThread;
    private bool _isConnected = false;

    // 메시지 큐 (메인 스레드에서 처리하기 위함)
    private readonly Queue<string> _receivedMessages = new Queue<string>();
    private readonly object _messageQueueLock = new object();

    void Start()
    {
        ConnectToServer();
    }
    #region 서버연결
    public void ConnectToServer()
    {
        if (_isConnected) return;

        try
        {
            _client = new TcpClient();
            _client.Connect(serverAddress, serverPort); // 서버에 연결 시도 (블로킹)
            _stream = _client.GetStream();
            _isConnected = true;
            Debug.Log("서버에 연결되었습니다.");

            // 메시지 수신을 위한 별도 스레드 시작
            _receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }
        catch (SocketException ex)
        {
            Debug.LogError($"소켓 연결 오류: {ex.Message}");
            _isConnected = false;
            // n초 뒤에 게임 끄기
        }
        catch (Exception ex)
        {
            Debug.LogError($"연결 오류: {ex.Message}");
            _isConnected = false;
            // n초 뒤에 게임 끄기
        }
    }
    private void ReceiveMessages()
    {
        byte[] buffer = new byte[4096]; // 수신 버퍼 크기
        int bytesRead;

        try
        {
            while (_isConnected && _stream != null && _stream.CanRead)
            {
                bytesRead = 0;
                try
                {
                    bytesRead = _stream.Read(buffer, 0, buffer.Length); // 메시지 수신 (블로킹)
                }
                catch (System.IO.IOException ioEx)
                {
                    Debug.LogError($"스트림 읽기 오류 (IO): {ioEx.Message}");
                    // 연결이 끊어졌을 가능성이 높음
                    _isConnected = false;
                    break;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"메시지 수신 중 오류: {ex.Message}");
                    _isConnected = false; // 오류 발생 시 연결 상태 false로 변경
                    break;
                }


                if (bytesRead == 0)
                {
                    // 서버가 연결을 정상적으로 종료함
                    Debug.Log("서버 연결이 끊겼습니다.");
                    _isConnected = false;
                    break;
                }

                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                lock (_messageQueueLock)
                {
                    _receivedMessages.Enqueue(receivedMessage);
                }
            }
        }
        catch (ThreadAbortException)
        {
            Debug.Log("수신 스레드가 중단되었습니다.");
        }
        catch (Exception ex)
        {
            if (_isConnected) // 연결된 상태에서만 오류 로그 출력
            {
                Debug.LogError($"수신 스레드 오류: {ex.Message}");
            }
        }
        finally
        {
            _isConnected = false; // 어떤 이유로든 루프를 빠져나오면 연결 끊김으로 처리
            Debug.Log("수신 스레드 종료.");
        }
    }
    #endregion 서버연결
    void Update()
    {
        // 메인 스레드에서 수신된 메시지 처리
        lock (_messageQueueLock)
        {
            while (_receivedMessages.Count > 0)
            {
                string message = _receivedMessages.Dequeue();
                Debug.Log("서버로부터 수신: " + message);
                // 여기서 수신된 메시지를 기반으로 게임 로직 처리
                // 예: if (message == "PLAYER_JUMP") { /* 플레이어 점프 로직 */ }
            }
        }

        // 테스트용: 'S' 키를 누르면 서버로 메시지 전송
        // if (Input.GetKeyDown(KeyCode.S) && _isConnected)
        // {
        //     byte[] buffer = Encoding.UTF8.GetBytes("Hello Server from Unity!");
        //     SendMessageToServer(buffer);
        // }
    }
    #region 서버보냄
    public void SendMessageToServer(byte[] messageData)
    {
        if (!_isConnected || _stream == null || !_stream.CanWrite)
        {
            Debug.Log("서버에 연결되지 않았거나 스트림을 사용할 수 없어 다시 서버 연결");
            // 로딩 같은 화면 대체
            this.ConnectToServer();
            return;
        }

        try
        {
            _stream.Write(messageData, 0, messageData.Length);
            _stream.Flush();
            Debug.Log("서버로 메시지 전송: " + messageData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"메시지 전송 오류: {ex.Message}");
            _isConnected = false; // 전송 오류 시 연결 끊김으로 간주
            Debug.Log("메세지 오류는 연결 끊김으로 간주해서 다시 서버연결");
            this.ConnectToServer();
        }
    }
    #endregion 서버보냄
    #region 서버해제
    void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public void DisconnectFromServer()
    {
        _isConnected = false; // 먼저 플래그를 설정하여 수신 스레드가 종료되도록 유도

        if (_receiveThread != null && _receiveThread.IsAlive)
        {
            // 스레드가 즉시 종료되지 않을 수 있으므로 Join으로 대기하거나,
            // IsBackground = true로 설정했으므로 애플리케이션 종료 시 자동으로 정리되도록 둘 수 있습니다.
            // _receiveThread.Abort(); // Abort는 가급적 피하는 것이 좋습니다.
            // _receiveThread.Join(); // 필요하다면 Join으로 대기
        }

        if (_stream != null)
        {
            _stream.Close();
            _stream = null;
        }
        if (_client != null)
        {
            _client.Close();
            _client = null;
        }
        Debug.Log("서버 연결이 종료되었습니다.");
    }
    #endregion 서버해제
}
