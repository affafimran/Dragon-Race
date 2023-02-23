using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Networking;

[Serializable]
public class PlayerVariables
{
	public string user;
	public string amount;
	public long sigTime;
	public string sig;
}

public class PlayerData : MonoBehaviour
{
	private Main main;

	public PlayerVariables playerVariables;

    private void Awake()
    {
		
    }

    void Start()
	{
		playerVariables = new PlayerVariables();

		main = GetComponent<Main>();
		//fetch data from Json of all players
		//StartCoroutine(GetPlayers(main.AllPlayerDataURL));
		//fetch data from Json of top ten players
		//StartCoroutine(GetPlayers(main.TopTenPlayerDataURL));
		//fetch data from Json of Individual player
		//StartCoroutine(GetPlayers(main.IndividualPlayerDataURL));

		//GetPlayerData();

		//StartCoroutine(GetPlayers(main.PostPlayerDataURL));

		//playerVariables.address = Individual.address;
		//playerVariables.Score = 5;
		//playerVariables.lives = Individual.lives;
		//playerVariables.player = Individual.player;


		//string playerJson = JsonUtility.ToJson(playerVariables);

		//System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", playerJson);

		//StartCoroutine(GetPlayers(main.AllPlayerDataURL));

		
	}

    private void Update()
    {
        
    }

    
    public void GetPlayerData()
    {
		StartCoroutine(GetPlayers(main.IndividualPlayerDataURL));

		

		Debug.Log("Get player data");
	}

	public void UpdateScore()
    {
		StartCoroutine(UpdateData());
	}

	IEnumerator UpdateData()
    {
		WWWForm form = new WWWForm();
		string jsonString = JsonUtility.ToJson(playerVariables);

		form.AddField("x", jsonString);

		WWW wWW = new WWW(main.PostPlayerDataURL, form);
		yield return null;
    }

	public void CheckWallet()
    {
		
    }

	//***************************************************
	IEnumerator GetPlayers(string url)
	{
		//string url = main.AllPlayerDataURL;
		//Debug.Log("")

		UnityWebRequest request = UnityWebRequest.Get(url);
		request.chunkedTransfer = false;
		yield return request.Send();

		if (request.isNetworkError)
		{
			//show message "no internet "
		}
		else
		{
			if (request.isDone)
			{
				if(url == main.AllPlayerDataURL)
                {
					
				}
				else if (url == main.TopTenPlayerDataURL)
				{
					
				}
				else if (url == main.IndividualPlayerDataURL)
				{
					

				}

			}
		}
	}	
}
