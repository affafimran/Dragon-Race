using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;
using Fusion.Sockets;
using System;

public class NetworkSceneManager : NetworkSceneManagerBase
{
    public static NetworkSceneManager Instance { get; private set; }

    public NetworkRunner runner;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public const int scene_Menu = 1;
    
    public bool goToMenu;

    public void Start()
    {
        //Invoke("Shutdown", 5f);
    }

    void Shutdown()
    {
        runner.Shutdown();
    }

    public void LoadMenu()
    {
        Destroy(NetworkSceneManager.Instance.runner.gameObject);
        SceneManager.LoadSceneAsync(scene_Menu);
    }
    
    public void DisconnectFromMatch()
    {
        //goToMenu = true;

        //Debug.Log($"OnShutdown Network players count: {NetworkPlayer.networkPlayers.Count}");
        //RaceSceneManager.instance.ToggleUIPanel(RaceSceneManager.instance.panel_Loadingimage, true);

        //runner.Shutdown();    

        runner.SetActiveScene(scene_Menu);
        Destroy(runner);

        LoadMenu();
        //if (RaceSceneManager.instance.networkPlayer_Winner == NetworkPlayer.Local && RaceSceneManager.instance.networkPlayer_Winner != null)
        //{
        //    LoadMenu();
        //}
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {
        Debug.Log($"Loading scene {newScene}");

        
        List<NetworkObject> sceneObjects = new List<NetworkObject>();

        if (newScene >= 0)
        {
            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Single);
            Scene loadedScene = SceneManager.GetSceneByBuildIndex(newScene);
            Debug.Log($"Loaded scene {newScene}: {loadedScene}");
            sceneObjects = FindNetworkObjects(loadedScene, disable: false);
        }
        

        finished(sceneObjects);

        // Delay one frame, so we're sure level objects has spawned locally
        yield return null;
    }

}
