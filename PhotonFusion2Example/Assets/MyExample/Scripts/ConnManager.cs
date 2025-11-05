using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.OnScreen;
using TMPro;

public class ConnManager : Fusion.Behaviour, INetworkRunnerCallbacks
{
    public static ConnManager instance;
    void Awake()
    {
        // ConnManager를 이 프로젝트에서 단 한 개만 생성
        // Singleton Pattern
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Screen.SetResolution(800, 600, FullScreenMode.Windowed);
        runner = GetComponent<NetworkRunner>();
        runner.ProvideInput = true;
    }

    NetworkRunner runner;
    public string userNickname;
    public TMPro.TMP_InputField inputFieldNickname;


    async void StartGame(GameMode mode, string sessionName)
    {
        userNickname = inputFieldNickname.text;
        
        SceneRef scene = SceneRef.FromIndex(1);
        NetworkSceneInfo sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(scene, LoadSceneMode.Single);

        // Fusion Server로 접속
        //StartGameArgs sga = new StartGameArgs();
        //sga.GameMode = GameMode.AutoHostOrClient;
        //sga.Scene = scene;
        //sga.SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>();
        //await runner.StartGame(sga);

        //await runner.StartGame(new StartGameArgs()
        //{
        //    GameMode = GameMode.AutoHostOrClient,
        //    SessionName = "TestRoom",
        //    Scene = scene,
        //    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        //});

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    void Update()
    {

    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData data = new NetworkInputData();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        data.mouseX = Input.GetAxis("Mouse X");

        data.direction = new Vector3(h, 0, v);  

        input.Set(data);
    }

    Dictionary<PlayerRef, NetworkObject> spawnedPlayerList = new Dictionary<PlayerRef, NetworkObject>();
    public GameObject playerFactory;
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            print("OnPlayerJoined : " + player.PlayerId.ToString());
            Vector3 point = UnityEngine.Random.insideUnitSphere * 5f;
            point.y = 0;
            NetworkObject netObj = runner.Spawn(playerFactory, point, Quaternion.identity, player);
            spawnedPlayerList.Add(player, netObj);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // 플레이어 리스트(spawnedPlayerList)에서 나간 플레이어(player)를 찾아 파괴
        if(spawnedPlayerList.TryGetValue(player, out NetworkObject netObj))
        {
            runner.Despawn(netObj);
            spawnedPlayerList.Remove(player);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public TMP_InputField inputFieldSessionName;
    public void OnClickCreateSession()
    {
        string name = inputFieldSessionName.text;
        // name으로 방 생성 후 Host로 StartGame 실행
        StartGame(GameMode.Host, name);
    }

    public void OnClickSearchSession()
    {
        runner.JoinSessionLobby(SessionLobby.ClientServer);
    }

    public Transform content;
    public RoomInfoUI roomInfoFactory;
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // 기존 content에 자식이 존재하면 모두 파괴
        int count = content.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        foreach (var info in sessionList)
        {
            RoomInfoUI roomInfoUI = Instantiate<RoomInfoUI>(roomInfoFactory);
            roomInfoUI.Init(info);
            roomInfoUI.transform.parent = content;
        }
    }

    public void JoinSession(string joinSessionName)
    {
        StartGame(GameMode.Client, joinSessionName);
    }
}
