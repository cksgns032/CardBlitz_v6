using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using UnityEngine;

public class SncyTcp : SingleTon<SncyTcp>
{
    // --- 설정 변수 ---
    [Header("Server Connection Settings")]
    [SerializeField] private string serverIp = "192.168.219.103"; // 서버 IP 주소 (예: 로컬 호스트)
    [SerializeField] private int serverPort = 8888;      // 서버 포트

    // --- 내부 변수 ---
    private TcpClient client;
    private NetworkStream stream;
    private CancellationTokenSource cancellationTokenSource; // 비동기 작업 취소를 위한 토큰
    private ObserverManager observerManager;

    // --- 유니티 콜백 메서드 ---

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 소켓 닫기
        Disconnect();
        Application.quitting -= OnApplicationQuitting;
    }

    private void OnApplicationQuitting()
    {
        // 애플리케이션 종료 시 호출
        Disconnect();
    }

    // --- 클라이언트 연결 및 데이터 송수신 메서드 ---

    public async void ConnectToServer()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        serverIp = ipHostInfo.AddressList[1].ToString();

        if (client != null && client.Connected)
        {
            Debug.LogWarning("Already connected to server.");
            return;
        }

        try
        {
            client = new TcpClient();
            Debug.Log($"Attempting to connect to {serverIp}:{serverPort}...");

            // 비동기 연결
            await client.ConnectAsync(serverIp, serverPort);
            stream = client.GetStream();
            Debug.Log("Successfully connected to server!");

            // 연결 성공 후 데이터 수신 루프 시작
            cancellationTokenSource = new CancellationTokenSource();
            _ = ReceiveLoopAsync(cancellationTokenSource.Token); // Task를 기다리지 않고 실행
        }
        catch (SocketException e)
        {
            Debug.LogError($"Socket Error: {e.Message}. Error Code: {e.SocketErrorCode}");
            Disconnect();
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection Error: {e.Message}");
            Disconnect();
        }
    }

    public async void SendMessage(byte[] message)//string message
    {
        if (client == null || !client.Connected || stream == null)
        {
            Debug.LogWarning("Not connected to server. Cannot send message.");
            return;
        }

        try
        {
            await stream.WriteAsync(message, 0, message.Length);
        }
        catch (Exception e)
        {
            Debug.LogError($"Send Error: {e.Message}");
            Disconnect();
        }
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        byte[] buffer = new byte[1024]; // 1KB 버퍼
        StringBuilder receivedData = new StringBuilder();

        try
        {
            while (client.Connected && !cancellationToken.IsCancellationRequested)
            {
                // ReadAsync는 CancellationToken을 받아 외부에서 취소 가능
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                if (bytesRead == 0) // 서버에서 연결을 종료한 경우
                {
                    Debug.Log("Server disconnected.");
                    break;
                }

                if (observerManager != null)
                {
                    observerManager.Notify(buffer);
                }

                receivedData.Clear(); // 처리된 데이터 제거
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Receive loop cancelled.");
        }
        catch (SocketException e) when (e.SocketErrorCode == SocketError.ConnectionReset)
        {
            Debug.LogWarning("Server forcibly closed connection.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Receive Error: {e.Message}");
        }
        finally
        {
            // 수신 루프 종료 시 연결 해제
            Disconnect();
        }
    }

    public void Disconnect()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel(); // 수신 루프 취소 요청
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }

        if (stream != null)
        {
            stream.Close();
            stream.Dispose();
            stream = null;
        }

        if (client != null)
        {
            client.Close();
            client = null;
            Debug.Log("Disconnected from server.");
        }
    }

    void Start()
    {
        observerManager = ObserverManager.Instance;
        Application.quitting += OnApplicationQuitting;
        ConnectToServer();
    }
}