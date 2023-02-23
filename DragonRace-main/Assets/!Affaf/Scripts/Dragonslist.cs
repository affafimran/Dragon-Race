using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Dragonslist : MonoBehaviour
{
    public static Dragonslist Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("GameObjects")]

    public GameObject[] dragons;

    [Space]

    [Header("Variables")]

    public int selecteddragon = 0;
    public float rotationSpeed;

    [Space]

    [Header("UI")]

    public Text text_Name;
    public Text text_Description;
    public Text text_Speed;
    public Text text_Power;

    public Button[] button_Dragon;

    [Space]

    [Header("Other")]

    public DragonStats[] dragonStats;

    [Space]

    [Header("Camera")]

    public Camera camera_Dragon;

    private void Start()
    {
        if(PlayerPrefs.GetInt("Selecteddragon") == 0)
        {
            PlayerPrefs.SetInt("Selecteddragon", 1);
        }

        //GetDragonStatsfromPrefs();

        SelectDragon(PlayerPrefs.GetInt("Selecteddragon") - 1);
        //RPC_SetKartId(selecteddragon);
        UpdateDragonButtons();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            RotateObject();
        }
    }

    /// <summary>
    /// Rotate current selected UI Dragon
    /// </summary>
    void RotateObject()
    {
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;        
        // select the axis by which you want to rotate the GameObject
        dragons[selecteddragon].transform.Rotate(Vector3.down, XaxisRotation);
        
    }

    void UpdateDragonButtons()
    {
        for(int i = 0; i < button_Dragon.Length; i++)
        {
            button_Dragon[i].transform.GetComponentInChildren<Text>().text = dragonStats[i].DragonName;
            button_Dragon[i].transform.GetComponentInChildren<Text>().color = Color.white;
            button_Dragon[i].transform.GetComponentInChildren<Text>().resizeTextForBestFit = true;
            button_Dragon[i].transform.GetChild(0).transform.localScale = new Vector3(0.9f, 0.8f, 1f);

        }
    }

    public void selectdragon()
    {
        PlayerPrefs.SetInt("Selecteddragon", selecteddragon);
        
    }

    public void nextdragon()
    {
        dragons[selecteddragon].SetActive(false);
        selecteddragon = (selecteddragon + 1) % dragons.Length;
        dragons[selecteddragon].SetActive(true);
    }



    public void Previousdragon()
    {
        dragons[selecteddragon].SetActive(false);
        selecteddragon--;
        if (selecteddragon < 0)
        {
            selecteddragon += dragons.Length;
        }
        dragons[selecteddragon].SetActive(true);
    }

    #region Dragon Selection
    /// <summary>
    /// Disable all dragon prefabs
    /// </summary>
    public void DisableDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.SetActive(false);
        }

        foreach (var button in button_Dragon)
        {
            //button.GetComponent<TweenScale>().ResetToBeginning();
        }
    }

    /// <summary>
    /// Set current select dragon to Selected
    /// </summary>
    /// <param name="selected"></param>
    public void SelectDragon(int selected)
    {
        DisableDragons();
        dragons[selected].SetActive(true);

        SetAttributes(selected);

        selecteddragon = selected;
        RPC_SetKartId(selecteddragon);

        //PlayerPrefs.SetInt("Selecteddragon", selected);

        if (GameManager.instance.screen == GameManager.MenuScreen.HallOfDragons)
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_DragonSelect);

    }

    #endregion

    #region UI Callbacks
    /// <summary>
    /// Set current selected dragon's attributes on UI
    /// </summary>
    /// <param name="dragon"></param>
    public void SetAttributes(int dragon)
    {
        text_Name.text = dragonStats[dragon].DragonName;
        text_Description.text = dragonStats[dragon].Description;
        text_Speed.text = dragonStats[dragon].Speed.ToString();
        text_Power.text = dragonStats[dragon].Power.ToString();
    }

    public void info_Dragons()
    {

    }

    public void info_Attributes()
    {

    }

    public void info_Skills()
    {

    }

    public void SpeedUp()
    {

    }

    #endregion

    #region Train Dragons

    /// <summary>
    /// Train Dragon Button
    /// </summary>
    public void TrainDragon()
    {
        int dragon = selecteddragon;
        int coins = PlayerPrefs.GetInt("Coins");
        if (coins >= 50)
        {
            dragonStats[dragon].Speed += 1;
            dragonStats[dragon].Power += 1;
            

            GameManager.instance.DeductCoins(50);
            dragons[dragon].GetComponent<Dragonstats>().SetStats(dragonStats[dragon].Speed, dragonStats[dragon].Power, dragonStats[dragon].Key);
            SetAttributes(dragon);

            APIManager.Instance.dragons[dragon].power = dragonStats[dragon].Power;
            APIManager.Instance.dragons[dragon].speed = dragonStats[dragon].Speed;

            APIManager.Instance.dragonAttributesForPUT.speed = dragonStats[dragon].Speed;
            APIManager.Instance.dragonAttributesForPUT.power = dragonStats[dragon].Power;            

            APIManager.Instance.DragonStats_PUT();
            APIManager.Instance.PlayerCoins_PUT();
        }
        else
        {
            //show notification
            Debug.LogError($"Not Enough coins for training");
        }

        
    }

    /// <summary>
    /// Get dragons stats from playerprefs if one exists
    /// </summary>
    public void GetDragonStatsfromPrefs()
    {
        for(int i = 0; i < dragonStats.Length; i++)
        {
            if (PlayerPrefs.HasKey($"{dragonStats[i].Key} Speed"))
            {
                dragonStats[i].Speed = PlayerPrefs.GetFloat($"{dragonStats[i].Key} Speed");
                dragonStats[i].Power = PlayerPrefs.GetFloat($"{dragonStats[i].Key} Power");
            }
            else
            {
                PlayerPrefs.SetFloat($"{dragonStats[i].Key} Speed", dragonStats[i].Speed);
                PlayerPrefs.SetFloat($"{dragonStats[i].Key} Power", dragonStats[i].Power);

                Debug.Log($"Dragon speed {PlayerPrefs.GetFloat($"{dragonStats[i].Key} Speed")} for {dragonStats[i].Key}");
            }
        }

        APIManager.Instance.GetDragonStats();
    }

    public void GetDragonStatsFromAPIManager()
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

        APIManager.Instance.GetDragonStats();
    }

    #endregion

    /// <summary>
    /// RPC: Set current selected Dragon's ID across network
    /// </summary>
    /// <param name="id"></param>
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetKartId(int id)
    {     
        PlayerPrefs.SetInt("Selecteddragon", id + 1);
    }
}
