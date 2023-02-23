using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Fusion;

public class CharacterInputHandler : MonoBehaviour
{
    public static CharacterInputHandler instance;

    Vector2 viewInputVector = Vector2.zero;

    public bool isreadytogo = false ;

    [Space]

    private Vector3 _Position;
  

    //Other components
    CharacterMovementHandler characterMovementHandler;
    private void Awake()
    {
        instance = this;
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //View input

        //viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look

        //characterMovementHandler.SetViewInputVector(viewInputVector);

        //_Position = transform.position;        
    }

    /// <summary>
    /// Check if all player have set their state to ready and start the race
    /// </summary>
    public void CheckForRaceStart()
    {
        if (RaceSceneManager.IsAllReady())
        {
            CryptoManager.Instance.StartNewGame.StartGame();
            //StartRace();
        }
    }

    public void StartRace()
    {
        RaceSceneManager.instance.ToggleUIPanel(RaceSceneManager.instance.panel_StartRace, false);

        for (int i = 0; i < NetworkPlayer.networkPlayers.Count; i++)
        {
            //NetworkPlayer.networkPlayers[i].GetComponent<SplineFollower>().spline = RaceSceneManager.instance.Path[i];
            //NetworkPlayer.networkPlayers[i].GetComponent<SplineFollower>().follow = true;
            NetworkPlayer.networkPlayers[i].RaceStarted(RaceSceneManager.instance.Path[i]);
        }
        RaceSceneManager.instance.Reward = NetworkPlayer.networkPlayers.Count * 50;

        //Set coins to be updated in a reserve Wallet Data
        APIManager.Instance.reserveCoins = (int)(RaceSceneManager.instance.Reward * 0.1f);

        //if (APIManager.Instance.totalCoins > 50)
        //{
        //    APIManager.Instance.totalCoins -= 50;
        //    PlayerPrefs.SetInt("Coins", (int)APIManager.Instance.totalCoins);
        //    APIManager.Instance.PlayerCoins_PUT();
        //}

        //foreach (var players in NetworkPlayer.networkPlayers)
        //{
        //    players.GetComponent<SplineFollower>().follow = true;
        //}

        //gameObject.GetComponent<SplineFollower>().follow = true;              //my uncomment
    }

    /// <summary>
    /// Set input value to send across network
    /// </summary>
    /// <returns></returns>
    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        //View data
        //networkInputData.rotationInput = viewInputVector.x;

        networkInputData.isready = isreadytogo;
        networkInputData.Position = transform.position;


        //reset 
        //isreadytogo = false;         //my comment

        return networkInputData;
    }
}
