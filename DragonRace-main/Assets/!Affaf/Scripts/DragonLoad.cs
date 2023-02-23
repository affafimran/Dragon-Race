using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonLoad : MonoBehaviour
{

    public GameObject[] Dragons;
    [HideInInspector] public GameObject currentDragon;

    public DragonStats[] dragonStats;

    [Space]

    public float current_Speed;
    public float current_Power;

    // Start is called before the first frame update
    void Start()
    {
        //int selecteddragon = PlayerPrefs.GetInt("Selecteddragon");
        //GameObject prefab = Dragons[selecteddragon];
        //Dragons[selecteddragon].SetActive(true);
        //FillDragonStatsfromPrefs();
    }


    public void LoadDragon(int id)
    {
        foreach (var dragon in Dragons)
        {
            dragon.SetActive(false);
        }
        Dragons[id - 1].SetActive(true);
        Debug.Log("dragon Playerpref: " + PlayerPrefs.GetInt("Selecteddragon"));
        currentDragon = Dragons[id - 1];

        //Disable Loading UI
        if (RaceSceneManager.instance.panel_Loadingimage.activeInHierarchy)
        {
            Debug.Log("Disable loader");
            RaceSceneManager.instance.panel_Loadingimage.SetActive(false);
            RaceSceneManager.instance.panel_StartRace.SetActive(true);
        }

    }

    public void FillDragonStatsfromPrefs()
    {
        for (int i = 0; i < dragonStats.Length; i++)
        {
            int entry = i + 1;
            dragonStats[i].Key = $"Dragon-{entry}";
            dragonStats[i].Speed = PlayerPrefs.GetFloat($"{dragonStats[i].Key} Speed");
            dragonStats[i].Power = PlayerPrefs.GetFloat($"{dragonStats[i].Key} Power");

            Debug.Log($"Dragon speed {PlayerPrefs.GetFloat($"{dragonStats[i].Key} Speed")} for {dragonStats[i].Key}");
        }
    }

    public void FillDragonStatsfromAPI()
    {
        for(int i = 0; i < dragonStats.Length; i++)
        {
            int entry = i + 1;
            dragonStats[i].Key = $"Dragon-{entry}";
            dragonStats[i].DragonName = APIManager.Instance.playerDataGET.player.dragons[i].dragonName;
            dragonStats[i].Description = APIManager.Instance.playerDataGET.player.dragons[i].description;
            dragonStats[i].Speed = APIManager.Instance.playerDataGET.player.dragons[i].speed;
            dragonStats[i].Power = APIManager.Instance.playerDataGET.player.dragons[i].power;
        }
    }

    public void UpdateDragonStats(float speed, float power)
    {
        //if (GetComponent<NetworkPlayer>() == NetworkPlayer.Local)
        //{
        //    Debug.Log($"Set local player stats");
        //    GetComponent<SplineFollower>().followSpeed = dragonStats[PlayerPrefs.GetInt("Selecteddragon") - 1].Speed;
        //    NetworkPlayer.Local.Speed = dragonStats[PlayerPrefs.GetInt("Selecteddragon") - 1].Speed;
        //    NetworkPlayer.Local.Power = dragonStats[PlayerPrefs.GetInt("Selecteddragon") - 1].Power;
        //    NetworkPlayer.Local.RPC_SetDragonStats(dragonStats[PlayerPrefs.GetInt("Selecteddragon") - 1].Speed, dragonStats[PlayerPrefs.GetInt("Selecteddragon") - 1].Power);
        //}
        //else
        //{
        //    Debug.Log($"Set other player stats");
        //    GetComponent<SplineFollower>().followSpeed = GetComponent<NetworkPlayer>().Speed;
        //}
        
        GetComponentInParent<SplineFollower>().followSpeed = speed;
        Debug.Log($"Dragons Speed updated {speed}");
    }

}
