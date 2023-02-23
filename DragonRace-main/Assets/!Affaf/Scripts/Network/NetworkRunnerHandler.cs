using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;
using Dreamteck.Splines;
using Fusion.Samples.HostMigration;
using Fusion.Photon.Realtime;

public class NetworkRunnerHandler : MonoBehaviour
{
    public static NetworkRunnerHandler Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
        mapPlayerToken = new Dictionary<int, NetworkPlayer>();

        connectionToken = ConnectionTokenUtils.NewToken();
    }

    [Header("Prefabs")]

    public NetworkRunner networkRunnerPrefab;   

    [Space]

    [Header("References")]

    public NetworkRunner networkRunner;
    private Camera main;

    [Space]

    [Header("Rooms Stats")]

    [SerializeField] private string roomName;
    [SerializeField] private string roomBet;

    public bool HostMigrate;

    [Space]

    public int maxPlayerCount;

    [Space]

    [Header("Other")]

    public Dictionary<int, NetworkPlayer> mapPlayerToken;

    public byte[] connectionToken;

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        
        main = Camera.main;
        
        Initialize();
    }

    public async void Initialize()
    {
        maxPlayerCount = PlayerPrefs.GetInt("PlayerCount");

        RaceSceneManager.instance.ToggleUIPanel(RaceSceneManager.instance.panel_Loadingimage, true);
        GetRoomStats();

        var properties = new Dictionary<string, SessionProperty>();
        properties.Add("bet", roomBet);

        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner";
        NetworkSceneManager.Instance.runner = networkRunner;

        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, connectionToken, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        await clientTask;


        //networkRunner.LobbyInfo.IsValid = true;       
        Debug.Log($"Server {roomName} NetworkRunner started");
        Debug.Log($"Lobby {networkRunner.LobbyInfo.IsValid} NetworkRunner started");
        RaceSceneManager.instance.text_SessionName.text = "ROOM: " + roomName;
       
    }

    /// <summary>
    /// Get room stats like room name, player count etc
    /// </summary>
    void GetRoomStats()
    {
        roomName = PlayerPrefs.GetString("RoomName");
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + UnityEngine.Random.Range(0, 10000).ToString();
        }
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode,byte[] connectiontoken, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            //Handle networked objects that already exits in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = roomName,
            Initialized = initialized,
            SceneManager = sceneManager,
            PlayerCount = maxPlayerCount,
            ConnectionToken = connectiontoken,
            CustomLobbyName = "DEFAULT",
        });
    }

    #region Network Token
    public byte[] GetConnectionToken()
    {
        Debug.Log($"Inside Get connection token");
        return connectionToken;
    }

    public void SetConnectionToken(byte[] connectionToken)
    {
        this.connectionToken = connectionToken;
    }

    public int GetPlayerToken(NetworkRunner runner, PlayerRef playerRef)
    {
        if (runner.LocalPlayer == playerRef)
        {
            Debug.Log($"Inside Get local player token");
            return ConnectionTokenUtils.HashToken(connectionToken);
        }
        else
        {
            Debug.Log($"Inside Get connection token");
            var token = runner.GetPlayerConnectionToken(playerRef);

            if(token != null)
                return ConnectionTokenUtils.HashToken(token);

            return 0;
        }
    }

    public void SetConnectionTokenMapping(int token, NetworkPlayer networkPlayer)
    {
        mapPlayerToken.Add(token, networkPlayer);
        Debug.Log($"HM set token map {mapPlayerToken[token]}");
    }

    #endregion

    #region Host Migration

    public async void MigrateHost(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        HostMigrate = true;
        //main.gameObject.SetActive(true);
        Debug.Log($"HM Shutting down Old runner: " + runner.name);
        RaceSceneManager.instance.ShowDialoguePopUp(":Host Disconnected: \n Assigning new Host", false, true);

        // Shutdown the current Runner, this will not be used anymore. Perform any prior setup and tear down of the old Runner
        await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

        // Create a new Runner.
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network runner";
        NetworkSceneManager.Instance.runner = networkRunner;

        Debug.Log($"HM New runner created: " + networkRunner.name);

        Debug.Log($"HM Start new runner: " + networkRunner.name);

        //var sceneManager = networkRunner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
        StartGameResult result = await networkRunner.StartGame(new StartGameArgs()
        {
            // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
            // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
            //SceneManager = sceneManager,
            //Scene = SceneManager.GetActiveScene().buildIndex,
            HostMigrationToken = hostMigrationToken,   // contains all necessary info to restart the Runner
            HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
            ConnectionToken = connectionToken,
        });

        //Check result
        if(result.Ok == false)
        {
            Debug.LogWarning(result.ShutdownReason);
            RaceSceneManager.instance.ShowDialoguePopUp(":Host Migration Failed: \n Reason: " + result.ShutdownReason, true, false);
        }
        else
        {
            RaceSceneManager.instance.GameLog("New Host Created");
        }

        //On resuming host migration
        void HostMigrationResume(NetworkRunner runner)
        {
            RaceSceneManager.instance.ShowDialoguePopUp(":Host Migrated: \n Resuming Players State", false, true);
            foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())
            {
                Debug.Log($"HM runner GameMode {resumeNO}");

                if (resumeNO.TryGetBehaviour<CharacterMovementHandler>(out var _) && resumeNO.TryGetBehaviour<NetworkCharacterControllerPrototypeCustom>(out var nPlayer))
                {
                    Debug.Log($"HM get snapshot {resumeNO}");

                    //var pos = Vector3.zero;
                    //var rot = Quaternion.identity;

                    //foreach(var behaviour in resumeNO.NetworkedBehaviours)
                    //{
                    //    if(behaviour is NetworkCharacterControllerPrototypeCustom controller)
                    //    {
                    //        pos = controller.ReadPosition();
                    //        rot = controller.ReadRotation();
                    //        Debug.Log($"HM pos {pos} : rot {rot}");
                    //    }
                    //}

                    NetworkObject player = default;

                    player = runner.Spawn(resumeNO, position: nPlayer.ReadPosition(), rotation: nPlayer.ReadRotation(), onBeforeSpawned: (runner, player) =>
                    {
                        player.CopyStateFrom(resumeNO);

                        if(resumeNO.TryGetBehaviour<NetworkObject>(out var netPlayer))
                        {
                            FindObjectOfType<Spawner>().SetConnectionTokenMapping(netPlayer.GetComponent<NetworkPlayer>().Token, netPlayer);
                        }
                    });
                    //player.GetComponent<NetworkPlayer>().nickName = resumeNO.GetBehaviour<NetworkPlayer>().nickName;
                }
            }

            //Time.timeScale = 0f;
        }
    }

   

    #endregion
}
