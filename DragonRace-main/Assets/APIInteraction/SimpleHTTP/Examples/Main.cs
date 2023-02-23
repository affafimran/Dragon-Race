using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleHTTP;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

	private Text errorText;
	private Text successText;
	private string validURL = "https://jsonplaceholder.typicode.com/posts/";
	private string invalidURL = "https://jsonplaceholder.net/articles/";

	[Space]

	public string AllPlayerDataURL;
	public string IndividualPlayerDataURL;
	public string TopTenPlayerDataURL;
	public string PostPlayerDataURL;

	[Space]

	public string PutReserveWalletCoins;
	public string PutPlayerCoinsURL;
	public string PutDragonDataURL;
	public string PostWithDragonStatsURL;
	public string GetPlayerStatswithDragonsURL;

	[Space]

	public string PostClaimRewardURL;
	public string GetClaimRewardURL;

 //   [Space]

	//public string Address;
	//public int Lives;
	//public int Player;

	[Space]

	private PlayerVariables playerVariables;

	void Start () {		
		//playerVariables = GetComponent<PlayerData>().playerVariables;
	}

	IEnumerator Get(string baseUrl, int postId) {
		Request request = new Request (baseUrl);

		Client http = new Client ();
		yield return http.Send(request);
		ProcessResult(http);
	}

	IEnumerator GetStats()
	{
        Request request = new Request(GetPlayerStatswithDragonsURL + APIManager.Instance.playerWallet);

        Client http = new Client();
        yield return http.Send(request);
		APIManager.Instance.response_GETStats = http.Response().Body();
		APIManager.Instance.ConvertUserJSONData();
        ProcessResult(http);
    }

	IEnumerator GetClaimReward()
	{
        Request request = new Request(GetClaimRewardURL + APIManager.Instance.newClaimId.newClaimId.claimId);

        Client http = new Client();
        yield return http.Send(request);
        APIManager.Instance.response_GETClaimReward = http.Response().Body();
        APIManager.Instance.ConvertGetClaimRewardJSON();
        ProcessResult(http);
    }

	IEnumerator Post() {
		
		PlayerDataBody body = APIManager.Instance.playerData;

		Request request = new Request (PostPlayerDataURL).Post(RequestBody.From (body));

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator PostStats()
	{
        PlayerDataWithDragons body = APIManager.Instance.playerDataWithDragons;

        Request request = new Request(PostWithDragonStatsURL).Post(RequestBody.From(body));

        Client http = new Client();
        yield return http.Send(request);
		if (http.Response().Body().Contains("success"))
		{
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                RaceSceneManager.instance.panel_Dialogue.SetActive(false);
			}
			else
			{
                GameManager.instance.ShowNotification("New User Data Updated");
                GameManager.instance.DisableNotification(1f);
            }
            
        }
        ProcessResult(http);
    }

	IEnumerator PostEndRaceStats()
	{
        PostClaimReward body = APIManager.Instance.postClaimReward;

        Request request = new Request(PostClaimRewardURL).Post(RequestBody.From(body));

        Client http = new Client();
        yield return http.Send(request);
        if (http.Response().Body().Contains("success"))
        {
			APIManager.Instance.response_POSTClaimReward = http.Response().Body();
			APIManager.Instance.ConvertPOSTClaimRewardJSON();
        }
        ProcessResult(http);
    }

	IEnumerator UpdateDragonStats()
	{
        DragonAttributesForPUT body = APIManager.Instance.dragonAttributesForPUT;
        PutDragonDataURL = $"https://players-dragon.herokuapp.com/api/v1/players-dragons/update/{APIManager.Instance.playerWallet}/dragon/";
        Request request = new Request(PutDragonDataURL + Dragonslist.Instance.dragonStats[Dragonslist.Instance.selecteddragon].DragonName).Put(RequestBody.From(body));

        Client http = new Client();
        yield return http.Send(request);
        if (http.Response().Body().Contains("success"))
        {
            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                RaceSceneManager.instance.panel_Dialogue.SetActive(false);
            }
            else
            {
                GameManager.instance.ShowNotification("Dragon Data Updated");
                GameManager.instance.DisableNotification(1f);
            }

        }
        ProcessResult(http);
    }

	IEnumerator UpdatePlayerCoins()
	{
        CoinsForPUT body = APIManager.Instance.coinsForPUT;
        PutPlayerCoinsURL = $"https://players-dragon.herokuapp.com/api/v1/players-dragons/update/{APIManager.Instance.playerWallet}/totalCoins";
        Request request = new Request(PutPlayerCoinsURL).Put(RequestBody.From(body));

        Client http = new Client();
        yield return http.Send(request);
    }

	IEnumerator UpdateReserverWalletCoins()
	{
        ReserverWalletCoins body = APIManager.Instance.reserverWalletCoins;
        PutReserveWalletCoins = $"https://players-dragon.herokuapp.com/api/v1/players-dragons/update/{APIManager.Instance.playerWallet}/totalCoins";
        Request request = new Request(PutReserveWalletCoins).Put(RequestBody.From(body));

        Client http = new Client();
        yield return http.Send(request);
    }

	IEnumerator PostWithFormData() {
		
		FormData formData = new FormData().AddField("Test", "5");

		Request request = new Request (PostPlayerDataURL)
			.Post (RequestBody.From(formData));

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	//for updating values on server
	IEnumerator Put() {
		
        PlayerDataBody body = APIManager.Instance.playerData;

        Request request = new Request(PostPlayerDataURL)
            .Put(RequestBody.From<PlayerDataBody>(body));        

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator Delete() {
		Request request = new Request (validURL + "1")
			.Delete ();

		Client http = new Client ();
		yield return http.Send (request);
		ProcessResult (http);
	}

	IEnumerator ClearOutput() {
		yield return new WaitForSeconds (2f);
		//errorText.text = "";
		//successText.text = "";
	}

	void ProcessResult(Client http) {
		if (http.IsSuccessful ()) {
			Response resp = http.Response ();
			//successText.text = "status: " + resp.Status().ToString() + "\nbody: " + resp.Body();
			Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
		} else {
			//errorText.text = "error: " + http.Error();
		}
		StopCoroutine (ClearOutput ());
		StartCoroutine (ClearOutput ());
	}

    #region Update values on server
	public void UpdateValuesOnServer()
    {   
        StartCoroutine(Put());

		//UpdatePost();
    }

	/// <summary>
	/// Post players data with dragons Stats
	/// </summary>
	public void POST_Stats()
	{
		StartCoroutine (PostStats());
	}

	public void POST_CLaimReward()
	{
		StartCoroutine(PostEndRaceStats());
	}

	public void PUT_Attributes()
	{
		StartCoroutine(UpdateDragonStats());
	}

	public void PUT_Coins()
	{
		StartCoroutine(UpdatePlayerCoins());
	}

	public void PUT_ReserveWalletCoins()
	{
		StartCoroutine(UpdateReserverWalletCoins());
	}
    #endregion

    #region Get Values from Server
	public void GET_Stats()
	{
		StartCoroutine(GetStats());
	}

	public void GET_ClaimRewardResponse()
	{
		StartCoroutine(GetClaimReward());
	}
    #endregion

    public void GetPost() {
		//StartCoroutine (Get (validURL, 1));
		StartCoroutine (Get (IndividualPlayerDataURL, 1));
	}

	public void GetTopTenPlayerData()
    {
		StartCoroutine(Get(TopTenPlayerDataURL, 1));
	}

	public void GetIndividualPlayerData()
    {
		StartCoroutine(Get(IndividualPlayerDataURL, 1));
	}

	public void GetAllPlayerData()
    {
		StartCoroutine(Get(AllPlayerDataURL, 1));
	}

	public void UpdatePlayer()
    {
		StartCoroutine(PostWithFormData());
	}

	public void CreatePost() {
		StartCoroutine (Post ());
	}

	public void UpdatePost() {
		StartCoroutine (Put ());
	}

	public void DeletePost() {
		StartCoroutine (Delete ());
	}

	public void GetNonExistentPost() {
		StartCoroutine (Get (validURL, 999));
	}

	public void GetInvalidUrl() {
		StartCoroutine (Get (invalidURL, 1));
	}

	public void CreatePostWithFormData() {
		StartCoroutine (PostWithFormData ());
	}
}
