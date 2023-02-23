using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;            
        }
        else
        {
            //Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public Main main;

    [Space]
    public string playerWallet;
    public string showWallet;
    public float totalCoins;
    public string totalCoinsinWEI;
    public int reserveCoins;

    [Header("POST Tokens")]
    public string post_Amount;
    public string post_User;
    public long post_SigTime;
    public string post_Sig;

    [Header("GET Stats")]
    public string response_GETStats;
    public string response_GETClaimReward;
    public string response_POSTClaimReward;

    [Space]

    public PlayerDataBody playerData;
    public PlayerDataWithDragons playerDataWithDragons;
    public Root playerDataGET;

    public DragonStatsServer[] dragons;

    public DragonAttributesForPUT dragonAttributesForPUT = new DragonAttributesForPUT();
    public CoinsForPUT coinsForPUT = new CoinsForPUT();
    public ReserverWalletCoins reserverWalletCoins = new ReserverWalletCoins();
    public PostClaimReward postClaimReward = new PostClaimReward();
    public GetClaimRewardMessage getClaimReward = new GetClaimRewardMessage();
    public NewClaimIdMessage newClaimId = new NewClaimIdMessage();

    // Start is called before the first frame update
    void Start()
    {
        //PlayerData_GET();
        //PlayerCoins_PUT();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Convert in-game coins to tokens
    /// </summary>
    public void ConvertCoinsToTOkens()
    {
        post_User = RaceSceneManager.instance.PlayerWallet;
        post_Amount = (RaceSceneManager.instance.Reward / 100f).ToString();
        post_SigTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        post_Sig = "";
        playerData = new PlayerDataBody(post_User, post_Amount, post_SigTime, post_Sig);
        Debug.Log($"Player data to POST: {playerData}");
        main.UpdateValuesOnServer();
    }

    /// <summary>
    /// Fill API manager's dragon class
    /// </summary>
    public void GetDragonStats()
    {        
        for(int i = 0; i < dragons.Length; i++)
        {
            dragons[i].dragonName = Dragonslist.Instance.dragonStats[i].DragonName;
            dragons[i].description = Dragonslist.Instance.dragonStats[i].Description;
            dragons[i].speed = Dragonslist.Instance.dragonStats[i].Speed;
            dragons[i].power = Dragonslist.Instance.dragonStats[i].Power;
        }

        //PlayerData_POST();
        //PlayerData_GET();
    }

    public void ConvertPOSTClaimRewardJSON()
    {
        newClaimId = JsonUtility.FromJson<NewClaimIdMessage>(response_POSTClaimReward);
        //call get API
        ClaimReward_GET();
    }

    public void ConvertGetClaimRewardJSON()
    {
        getClaimReward = JsonUtility.FromJson<GetClaimRewardMessage>(response_GETClaimReward);
        if (string.IsNullOrEmpty(getClaimReward.claim.signature))
        {
            //Wait for 10 secs and call GET API again
            StartCoroutine(ClaimReward_GETwithDelay());
        }
        else
        {
            //you can call ClaimReward method
            Debug.Log("Signature recieved");
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                GameManager.instance.ShowNotification("Please Confirm To Finish Last Game");
                CryptoManager.Instance.claimReward.Claim();
            }
            else
            {
                RaceSceneManager.instance.ShowReward();
                //CryptoManager.Instance.claimReward.Claim();
            }

        }
    }

    public void ConvertUserJSONData()
    {
        if (response_GETStats.Contains("success"))
        {
            playerDataGET = JsonUtility.FromJson<Root>(response_GETStats);
            //totalCoins = playerDataGET.player.totalCoins;
            Dragonslist.Instance.GetDragonStatsFromAPIManager();
            GameManager.instance.DisableNotification(1f);
        }
        else if(response_GETStats.Contains("Not Found"))
        {
            Debug.LogError($"No Data found for this user");
            GameManager.instance.ShowNotification("New User Detected \n Creating Data for new User");
            //totalCoins = 1000;
            GetDragonStats();
            PlayerData_POST();
        }
    }

    public void PlayerData_GET()
    {
        GameManager.instance.ShowNotification("Fetching User Data");
        main.GET_Stats();
    }

    public void PlayerData_POST()
    {
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            RaceSceneManager.instance.ShowDialoguePopUp(":Notification: \n Updating User Data", false, true);
        }
        playerDataWithDragons = new PlayerDataWithDragons(playerWallet, totalCoins, dragons);
        main.POST_Stats();
    }

    public void PlayerCoins_POST()
    {
        playerDataWithDragons = new PlayerDataWithDragons(playerWallet, totalCoins, dragons);
        main.POST_Stats();
    }

    public void DragonStats_PUT()
    {
        main.PUT_Attributes();
    }

    public void PlayerCoins_PUT()
    {
        APIManager.Instance.coinsForPUT.totalCoins = APIManager.Instance.totalCoins;
        //APIManager.Instance.coinsForPUT.totalCoins = 1000;
        main.PUT_Coins();
    }

    public void ReserveWalletAmount_PUT()
    {
        reserverWalletCoins.fees = reserveCoins;
        main.PUT_ReserveWalletCoins();
    }

    #region Race END
    public void ClaimReward_POST(string reward)
    {
        postClaimReward.address = playerWallet;
        //for testing
        postClaimReward.reward = ConverttoWEI(reward);
        //postClaimReward.reward = ConverttoWEI(RaceSceneManager.instance.Reward.ToString());
        postClaimReward.cost = "0";
        main.POST_CLaimReward();
        Debug.Log("Claim reward POST called");
    }

    public void ClaimReward_GET()
    {
        main.GET_ClaimRewardResponse();
        Debug.Log("Claim reward GET called");
    }

    IEnumerator ClaimReward_GETwithDelay()
    {
        yield return new WaitForSeconds(10);
        ClaimReward_GET();
    }

    #endregion

    string ConverttoWEI(string value)
    {
        float eth = float.Parse($"{value}");
        float decimals = 1000000000000000000; // 18 decimals
        float wei = eth * decimals;

        string result = Convert.ToDecimal(wei).ToString();

        print("Value in WEI: " + Convert.ToDecimal(wei).ToString());
        return result;
    }
}

[Serializable]
public class PlayerDataBody
{
    public string user;
    public string amount;
    public long sigTime;
    public string sig;

    public PlayerDataBody(string user, string amount, long sigTime, string sig)
    {
        this.user = user;
        this.amount = amount;
        this.sigTime = sigTime;
        this.sig = sig;
    }
}

[Serializable]
public class PlayerDataWithDragons
{
    public string playersWalletAddress;
    public float totalCoins;
    public DragonStatsServer[] dragons;

    public PlayerDataWithDragons(string playersWalletAddress, float totalCoins, DragonStatsServer[] dragons)
    {
        this.playersWalletAddress = playersWalletAddress;
        this.totalCoins = totalCoins;
        this.dragons = dragons;
    }
}

[Serializable]
public class DragonStatsServer
{
    public string dragonName;
    public string description;
    public float speed;
    public float power;
}

public class CoinsForPUT
{
    public float totalCoins;
}

public class ReserverWalletCoins
{
    public int fees;
}

public class DragonAttributesForPUT
{
    public float speed;
    public float power;
}

[Serializable]
public class Root
{
    public string type;
    public Player player;
}
[Serializable]
public class Dragon
{
    public string dragonName;
    public string description;
    public int speed;
    public int power;
    public string _id;
}
[Serializable]
public class Player
{
    public string _id;
    public string playersWalletAddress;
    public int totalCoins;
    public List<Dragon> dragons;
    public string id;
}

/// <summary>
/// For POSTing data after race end
/// </summary>
[Serializable]
public class PostClaimReward
{
    public string address;
    public string reward;
    public string cost;
}

/// <summary>
/// To get Data from end race POST
/// </summary>
[Serializable]
public class NewClaimId
{
    public int claimId;
    public string address;
    public string reward;
    public string cost;
    public string sigTime;
    public string _id;
    public int __v;
    public string id;
}

[Serializable]
public class NewClaimIdMessage
{
    public string message;
    public NewClaimId newClaimId;
}

/// <summary>
/// To GET race end claim reward data, mainly signature
/// </summary>
[Serializable]
public class GetClaimReward
{
    public string _id;
    public int claimId;
    public string address;
    public string reward;
    public string cost;
    public string sigTime;
    public string signature;
    public string id;
}

[Serializable]
public class GetClaimRewardMessage
{
    public string type;
    public GetClaimReward claim;
}
