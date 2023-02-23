using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionsList : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner networkRunner;
    void Awake()
    {
        //Init();
    }

    async void Start()
    {
        
        //GameManager.instance.EnablePlayButton();
    }

    async void Init()
    {
        StartGameArgs startGameArgs = new StartGameArgs();
        startGameArgs.CustomLobbyName = "DEFAULT";
        startGameArgs.GameMode = GameMode.Shared;
        await networkRunner.StartGame(startGameArgs);
        Debug.Log($"Network runner created");
    }

    public void CreateGame()
    {
        StartGameArgs startGameArgs = new StartGameArgs();
        startGameArgs.CustomLobbyName = "DEFAULT";
        startGameArgs.SessionName = "Test";
        startGameArgs.GameMode = GameMode.Shared;
        networkRunner.StartGame(startGameArgs);
        Debug.Log($"Room created");
    }

    public void Leave()
    {
        networkRunner.Shutdown();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"OnConnected: {runner}");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"OnConnectionFailed: {reason}");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
       
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
       
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        var cachedRooms = sessionList.ToArray();
        Debug.Log($"OnSessionListUpdate: Total Session Count {cachedRooms.Length}");

        foreach (var sessionInfo in sessionList)
        {
            if (sessionList.Count > 0)
            {
                Debug.Log($"OnSessionListUpdate: Name {sessionInfo.Name} : Total Session Count {sessionList.Count}");
            }

        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"OnShutdown: {shutdownReason}");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

}
