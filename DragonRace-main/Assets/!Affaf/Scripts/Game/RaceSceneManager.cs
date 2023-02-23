using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using UnityEngine.UI;
using Dreamteck.Splines;
using static UnityEngine.Networking.UnityWebRequest;
using Fusion.Samples.HostMigration;

public class RaceSceneManager : MonoBehaviour
{
    public static RaceSceneManager instance { get; private set; }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    [Header("GameObjects")]

    public GameObject panel_Loadingimage;
    public GameObject panel_StartRace;
    public GameObject panel_RaceEnd;
    public GameObject panel_Dialogue;
    public GameObject panel_Winner;
    public GameObject panel_Loser;
    public GameObject panel_WaitingRaceEndAPI;
    public GameObject panel_WaitingClaimReward;

    [Space]

    [Header("Variables")]

    public string PlayerWallet;

    public bool b_StartRace;

    public int Reward;    
    public int TotalLaps;

    [Space]

    [Header("UI")]

    public Button button_StartRace;
    public Button button_Continue;
    public Button button_DialogueMenu;

    public Image image_WaitingForEnd;
    public Image img_WaitingForPlayers;

    public Text text_Dialogue;
    public Text text_DialogueWaiting;
    public Text text_GameLog;
    public Text text_SessionName;
    public Text text_JoinedCount;
    public Text text_Laps;
    public Text text_Reward;

    [Space]

    [Header("Other")]

    public SplineComputer[] Path;

    public EnteredPlayer enteredPlayerUI;

    public ResultUI resultUI;

    public GameLogs gameLogs;

    public NetworkPlayer networkPlayer_Winner;

    public MiniMapCam MiniMapCam;

    [Space]

    [Header("Race End")]

    public Transform t_StartRaceListParent;
    public Transform t_ResultListParent;
    public List<ResultUI> resultUIList;
    public List<EnteredPlayer> networkPlayersStart = new List<EnteredPlayer>();    

    // Start is called before the first frame update
    void Start()
    {
        //var test = ConnectionTokenUtils.NewToken();
        //foreach (var byt in test)
        //{
        //    Debug.Log($"ConnectionToken {((char)byt)}");
        //}
        PlayerWallet = PlayerPrefs.GetString("UserWallet");
        SoundManager.Instance.PlayBG(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callbacks

    public void GameLog(string message)
    {
        gameLogs.CreateMessage(message);
    }

    public void ShowDialoguePopUp(string text, bool menubuttonstate, bool waitingtextstate)
    {
        panel_Dialogue.SetActive(true);
        text_Dialogue.text = text;
        button_DialogueMenu.gameObject.SetActive(menubuttonstate);
        text_DialogueWaiting.gameObject.SetActive(waitingtextstate);
    }

    /// <summary>
    /// Disable all in-game UI panels
    /// </summary>
    public void DisableUI()
    {
        panel_Loadingimage.SetActive(false);
        panel_RaceEnd.SetActive(false);
        panel_StartRace.SetActive(false);
    }

    /// <summary>
    /// Set your dragon's state to ready
    /// </summary>
    public void button_GO()
    {

        NetworkPlayer.Local.RPC_SetStartRace(true);
        button_StartRace.interactable = false;
        button_StartRace.gameObject.SetActive(false);
        img_WaitingForPlayers.gameObject.SetActive(true);

        Debug.Log("network players Count: " + NetworkPlayer.networkPlayers.Count);
        //Debug.Log($"Player one start {NetworkPlayer.networkPlayers[0].StartRace} : Player two start {NetworkPlayer.networkPlayers[1].StartRace}");
        Debug.Log("IsAllReady => " + IsAllReady());
    }

    /// <summary>
    /// Toggle UI panel OFF or ON
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="state"></param>
    public void ToggleUIPanel(GameObject panel, bool state)
    {
        DisableUI();
        panel.SetActive(state);
    }

    public void button_ClaimReward()
    {
        CryptoManager.Instance.claimReward.Claim();
    }

    #endregion

    #region Network
    /// <summary>
    /// Bool to check if a all dragons have entered the current race/scene
    /// </summary>
    /// <returns></returns>
    public static bool IsAllEntered() => NetworkPlayer.networkPlayers.Count == NetworkRunnerHandler.Instance.maxPlayerCount && NetworkPlayer.networkPlayers.All(player => player.Entered);

    /// <summary>
    /// Bool to check if all the dragons/players has set their state to ready
    /// </summary>
    /// <returns></returns>
    public static bool IsAllReady() => NetworkPlayer.networkPlayers.Count == NetworkRunnerHandler.Instance.maxPlayerCount && NetworkPlayer.networkPlayers.All(player => player.StartRace);

    /// <summary>
    /// Bool to check if all the dragons/player have reached end of the race on their instance of the game
    /// </summary>
    /// <returns></returns>
    public static bool IsAllReachedEnd() => NetworkPlayer.networkPlayers.All(player => player.EndRace);

    #endregion

    #region Race

    public void SetupMiniMap(Transform player, GameObject indicator, bool isLocal)
    {
        if (isLocal)
        {
            MiniMapCam.player = player;
            indicator.GetComponent<SpriteRenderer>().color = Color.green;
            indicator.transform.localScale = Vector3.one * 50;
        }
        else
        {
            indicator.GetComponent<SpriteRenderer>().color = Color.red;
            indicator.transform.localScale = Vector3.one * 30;
        }
    }

    /// <summary>
    /// Update players list shown at the start of race
    /// </summary>
    public void UpdatePlayerList(NetworkPlayer networkPlayer, int entry, bool goState, bool playerLeft)
    {      
        //if Player has left the room/game
        if (playerLeft)
        {
            
            for (int i = 0; i < networkPlayersStart.Count; i++)
            {
                if (networkPlayersStart[i].PlayerName.text.Contains(networkPlayer.nickName.ToString()))
                {
                    Destroy(networkPlayersStart[i].gameObject);
                    networkPlayersStart.RemoveAt(i);
                }
                else
                {
                    networkPlayersStart[i].UpdateRanking(i + 1);
                }
               
            }
        }
        else
        {
            //if player already entered and set his state to GO
            if (goState)
            {
                if (networkPlayersStart[entry].PlayerName.text.Contains(networkPlayer.nickName.ToString()))
                {
                    networkPlayersStart[entry].ReadyMark.SetActive(true);
                    Debug.Log($"Update Go state {networkPlayer.nickName}");
                }
                else
                {
                    Debug.Log($"Failed to update Go state {networkPlayer.nickName}");
                }
            }
            else
            {
                Debug.Log($"Update Start list");
                var go = Instantiate(enteredPlayerUI, t_StartRaceListParent);
                go.SetPlayer(networkPlayer.nickName.ToString(), entry);

                networkPlayersStart.Add(go);                
            }
        }
        Debug.Log($"Player Limit {NetworkRunnerHandler.Instance.networkRunner.ActivePlayers.Count()}");
        text_JoinedCount.text = $"{NetworkPlayer.networkPlayers.Count}/{NetworkRunnerHandler.Instance.maxPlayerCount}";
    }

    /// <summary>
    /// Call When player has reached the end of race to show End Race UI and ranking.
    /// Ranking are calculated everytime a dragons completes the race, but UI will only show when your
    /// dragon reached the end.
    /// </summary>
    /// <param name="player"></param>
    public void PlayerReachedEnd(NetworkPlayer player)
    {
        //networkPlayers.Add(player);
        //networkPlayers.Sort();
        ClearResults();

        if (!panel_RaceEnd.activeInHierarchy && player.Object.HasInputAuthority)
            ToggleUIPanel(panel_RaceEnd, true);

        
        var dragons = GetFinishedDragons();

        Debug.Log("Finished Dragons: " + dragons.Count);

        resultUIList.Clear();

        for (int i = 0; i < dragons.Count; i++)
        {
            var resultPlayer = dragons[i];

            var result = Instantiate(resultUI, t_ResultListParent);
            result.SetResult(resultPlayer.nickName.ToString(), i + 1);

            resultUIList.Add(result);
        }

    }

    /// <summary>
    /// Check wheather to enable leave match button or not
    /// </summary>
    public void CheckLeaveButton()
    {
        button_Continue.gameObject.SetActive(IsAllReachedEnd());
        image_WaitingForEnd.gameObject.SetActive(!button_Continue.gameObject.activeInHierarchy);

        if (button_Continue.gameObject.activeInHierarchy)
        {
            panel_WaitingRaceEndAPI.SetActive(true);

            var dragons = GetFinishedDragons();

            if (NetworkRunnerHandler.Instance.maxPlayerCount == 2)
            {
                if (dragons[0].nickName == NetworkPlayer.Local.nickName)
                {
                    //winner, decided on fees of 2826 tokens
                    APIManager.Instance.ClaimReward_POST("5652");
                }
                else
                {
                    //loser
                    APIManager.Instance.ClaimReward_POST("0");
                }
            }

                    
        }
    }

    public void ShowReward()
    {
        panel_WaitingRaceEndAPI.SetActive(false);

        var dragons = GetFinishedDragons();

        //dragons[0].nickName = PlayerPrefs.GetString("Account");
        Debug.Log($"{dragons[0].nickName} am winner");
        if (NetworkRunnerHandler.Instance.maxPlayerCount == 2)
        {
            if (dragons[0].nickName == NetworkPlayer.Local.nickName)
            {
                //dragons[0].RewardUser(Reward);
                networkPlayer_Winner = dragons[0];
                resultUIList[0].WinnerMedal.SetActive(true);
                panel_Winner.gameObject.SetActive(true);
                Debug.Log($"Winner Coins " + PlayerPrefs.GetInt("Coins"));
            }
            else
            {
                resultUIList[0].WinnerMedal.SetActive(true);
                panel_Loser.gameObject.SetActive(true);
            }
        }
        if (NetworkRunnerHandler.Instance.maxPlayerCount > 2)
        {
            if (dragons[0].nickName == NetworkPlayer.Local.nickName)
            {
                Reward = (int)(Reward * 0.5f);
                dragons[0].RewardUser(Reward);
                networkPlayer_Winner = dragons[0];

                resultUIList[0].WinnerMedal.SetActive(true);
                resultUIList[1].SilverMedal.SetActive(true);
                resultUIList[2].BronzeMedal.SetActive(true);

                panel_Winner.gameObject.SetActive(true);

                Debug.Log($"Winner Coins " + PlayerPrefs.GetInt("Coins"));
            }
            else if (dragons[1].nickName == NetworkPlayer.Local.nickName)
            {
                Reward = (int)(Reward * 0.3f);
                dragons[1].RewardUser(Reward);

                resultUIList[0].WinnerMedal.SetActive(true);
                resultUIList[1].SilverMedal.SetActive(true);
                resultUIList[2].BronzeMedal.SetActive(true);

                panel_Winner.gameObject.SetActive(true);

                Debug.Log($"Second Coins " + PlayerPrefs.GetInt("Coins"));
            }
            else if (dragons[2].nickName == NetworkPlayer.Local.nickName)
            {
                Reward = (int)(Reward * 0.2f);
                dragons[2].RewardUser(Reward);

                resultUIList[0].WinnerMedal.SetActive(true);
                resultUIList[1].SilverMedal.SetActive(true);
                resultUIList[2].BronzeMedal.SetActive(true);

                panel_Winner.gameObject.SetActive(true);

                Debug.Log($"Third Coins " + PlayerPrefs.GetInt("Coins"));
            }
            else
            {
                resultUIList[0].WinnerMedal.SetActive(true);
                resultUIList[1].SilverMedal.SetActive(true);
                resultUIList[2].BronzeMedal.SetActive(true);
            }
        }

        text_Reward.text = Reward.ToString();
    }

    /// <summary>
    /// Get dragons that have reached the end of race on Local player's instance of the game
    /// </summary>
    /// <returns></returns>
    private static List<NetworkPlayer> GetFinishedDragons() => NetworkPlayer.networkPlayers.OrderBy(x => x.GetTotalRaceTime()).Where(dragon => dragon.EndRaceTick != 0).ToList();

    public void ClearResults()
    {  
        for (int i = 0; i < t_ResultListParent.childCount; i++)
        {
            Destroy(t_ResultListParent.GetChild(i).gameObject);
        }
    }

    #endregion

    #region API

    public void ConvertCoinsToTokens()
    {
        APIManager.Instance.ConvertCoinsToTOkens();
    }

    #endregion
}
