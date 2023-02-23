using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;



public class Spawner : SimulationBehaviour, INetworkRunnerCallbacks
{
    public NetworkPlayer playerPrefab;
    public NetworkPrefabRef prefabRef;

    //Dictionary<int, NetworkPlayer> mapPlayerToken;
    Dictionary<int, NetworkObject> mapPlayerToken;

    //Other compoents
    CharacterInputHandler characterInputHandler;     //my uncomment

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        mapPlayerToken = new Dictionary<int, NetworkObject>();
    }

    public void SetConnectionTokenMapping(int token, NetworkObject networkPlayer)
    {
        Debug.Log($"HM set token map network player {networkPlayer}");
        mapPlayerToken.Add(token, networkPlayer);        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Debug.Log($"OnPlayerJoined we are server");

            var playerObject = Runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);

            //Get token for player
            //int playerToken = NetworkRunnerHandler.Instance.GetPlayerToken(runner, player);

            ////Debug.Log($"Dictionary value network player {mapPlayerToken[playerToken]}");
            //Debug.Log($"OnPlayerJoined we are server. Connection token {playerToken}");
            
            ////check if token is already recorded in server
            //if (mapPlayerToken.TryGetValue(playerToken, out var networkPlayer))
            //{                
            //    Debug.Log($"Found Old connection token for {playerToken}");
                
            //    Debug.Log($"Old connection network player {networkPlayer}");
            //    /*NetworkObject nObject = */
            //    networkPlayer.GetComponent<NetworkObject>().AssignInputAuthority(player);

            //    //nObject.AssignInputAuthority(player);


            //}
            //else
            //{
            //    var playerObject = Runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);

            //    playerObject.Token = playerToken;

            //    //Store mapping player token and spawned object
            //    mapPlayerToken[playerToken] = playerObject.GetComponent<NetworkObject>();
            //    Debug.Log($"Set Dictionary value network player {mapPlayerToken[playerToken]}");
            //}

            //my code------------------------------
            if (!NetworkRunnerHandler.Instance.HostMigrate)
            {
                //var playerObject = Runner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
            }                

            //NetworkPlayer.networkPlayers.Add(playerObject);
            if (NetworkPlayer.Local != null)
            {
                //networkPlayer.DragonId = PlayerPrefs.GetInt("Selecteddragon");
                //NetworkPlayer.Local.RPC_SetDragonID(PlayerPrefs.GetInt("Selecteddragon"));
                //Debug.Log("Inside OnPlayer joined DragonId" + NetworkPlayer.Local.DragonId);
            }
            //-------------------------------------

            // Set Player Object to facilitate access across systems.
            //runner.SetPlayerObject(player, playerObject);            
        }
        else
        {
            Debug.Log("OnPlayerJoined");
        }
        
        //Invoke("delay", 1);
    }


    void delay()
    {
        Playerpathing.instance.setplayer();
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        //my uncomment------------------------------------
        if (characterInputHandler == null && NetworkPlayer.Local != null)
            characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();

        if (characterInputHandler != null)
            input.Set(characterInputHandler.GetNetworkInput());
        //------------------------------------------------
    }

    public void OnConnectedToServer(NetworkRunner runner) { Debug.Log("OnConnectedToServer"); }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
    { 
        Debug.Log("OnPlayerLeft");
        //if (player == NetworkPlayer.Local.Object.InputAuthority)
        //{
        //    var netPlayer = Object.GetComponent<NetworkPlayer>();
        //    Debug.Log($"OnPlayerLeft networkPlayer index: " + NetworkPlayer.networkPlayers.IndexOf(netPlayer));
        //    RaceSceneManager.instance.UpdatePlayerList(netPlayer, networkPlayers.IndexOf(netPlayer), false, true);
        //    Debug.Log($"Despawned: {netPlayer}");
        //    netPlayer.EndRaceTick = Runner.Simulation.Tick;
        //    networkPlayers.Remove(netPlayer);
        //    Debug.Log($"Remaining Players: {NetworkPlayer.networkPlayers.Count}");
        //    Destroy(netPlayer.ref_Collider);
        //    Runner.Despawn(Object);

        //    //If local player has reached end and a client leaves/disconnect, check for leave button again
        //    if (Laps == RaceSceneManager.instance.TotalLaps)
        //    {
        //        RaceSceneManager.instance.button_Continue.gameObject.SetActive(RaceSceneManager.IsAllReachedEnd());
        //        RaceSceneManager.instance.image_WaitingForEnd.gameObject.SetActive(!RaceSceneManager.instance.button_Continue.gameObject.activeInHierarchy);

        //    }

        //}
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"OnShutDown: {shutdownReason}");
        
        //if (NetworkSceneManager.Instance.runner)
        //{
        //    Destroy(NetworkSceneManager.Instance.runner.gameObject);
        //}
        NetworkPlayer.networkPlayers.Clear();
        //if(RaceSceneManager.instance.networkPlayer_Winner == NetworkPlayer.Local && RaceSceneManager.instance.networkPlayer_Winner != null) return;
        //if (RaceSceneManager.instance.button_Continue.gameObject.activeInHierarchy)
        //{
        //    return;
        //}
        
        NetworkSceneManager.Instance.DisconnectFromMatch();                //Temporary placed

        if (shutdownReason == ShutdownReason.HostMigration) return;

        if (shutdownReason == ShutdownReason.Ok)
        {
            //RaceSceneManager.instance.ShowDialoguePopUp(":Host Shutdown: \n Shutdown Reason: Self Shutdown", true, false);            
        }        
        else
        {
            //RaceSceneManager.instance.ShowDialoguePopUp(":Host Shutdown: \n Shutdown Reason: " + shutdownReason.ToString(), true, false);
        }
    }
    public void OnDisconnectedFromServer(NetworkRunner runner) { Debug.Log("OnDisconnectedFromServer"); }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { Debug.Log("OnConnectRequest"); }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { Debug.Log("OnConnectFailed"); }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) 
    {      
       
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //NetworkRunnerHandler.Instance.MigrateHost(runner, hostMigrationToken);         //Temporary commented
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
}
