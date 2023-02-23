using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fusion;

public class GameManager : MonoBehaviour
{
    
    public Text Announcertext;

    public static GameManager instance { get; private set; }

    public bool Greenenvironment;

    [Space]

    [Header("GameObjects")]

    public GameObject panel_Menu;
    public GameObject panel_Options;
    public GameObject panel_HallOfDragons;
    public GameObject panel_ChangeName;
    public GameObject panel_RoomPanel;
    public GameObject panel_WalletConnecting;

    public GameObject parent_Dragons;

    public GameObject panel_Notification;

    [Space]

    [Header("Other")]

    public MenuScreen screen;
    public Animator anim_MainCam;
    public Animator anim_DragonCam;

    [Space]

    [Header("Scripts")]

    public Dragonslist dragonslist;

    [Space]

    [Header("UI")]

    public InputField input_RoomName;

    [Space]

    public Button button_PlayerCount2;
    public Button button_PlayerCount5;

    public Button button_SoundON;
    public Button button_SoundOFF;

    [Space]

    public Text text_Coins;
    public Text text_Wallet;
    public Text text_Notification;

    [Space]

    public Button button_Play;
    public Image image_Connecting;

    [Header("Networking")]

    public NetworkRunner NetworkRunnerMenu;

    [Space]

    [Header("Variables")]

    public int CoinsMenu;

    public int SelectedMap;

    [Space]

    [Header("WEB3")]

    public GameObject panel_Approve;
    public GameObject panel_BuyTokens;

    [Space]

    public InputField if_BuyToken;

    [Space]

    public float BuyTokenAmount;
    public string buyTokenAmountInWEI;

    public enum MenuScreen
    {
        Menu,
        Options,
        HallOfDragons,
        ChangeName,
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        Debug.unityLogger.logEnabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetSoundButton();

        SoundManager.Instance.PlayBG(0);
        CoinsMenu = PlayerPrefs.GetInt("Coins");
        
        SelectPlayerCount(2);
        button_PlayerCount2.Select();

        //goto_Menu();
        //PlayerPrefs.SetInt("Coins", 5000);
        //if(PlayerPrefs.GetInt("Coins") < 50)
        //{
        //    APIManager.Instance.totalCoins = 1000;
        //    PlayerPrefs.SetInt("Coins", 1000);
        //}
       
    }

    private void Update()
    {
        text_Coins.text = APIManager.Instance.totalCoins.ToString();
        if (!string.IsNullOrEmpty(APIManager.Instance.playerWallet))
        {
            text_Wallet.text = APIManager.Instance.playerWallet;
        }
    }

    IEnumerator Turnofftext()
    {
        yield return new WaitForSeconds(1f);
        Announcertext.enabled = false;
    }

    public void goto_RoomPanel()
    {
        DisableUI();
        panel_RoomPanel.SetActive(true);
    }
       
    public void quit()
    {
        Application.Quit();
    }

    #region UI Callbacks
    public void ShowNotification(string text)
    {
        panel_Notification.SetActive(true);
        text_Notification.text = text;        
    }

    public void DisableNotification(float wait)
    {
        StartCoroutine(Wait(wait));        
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);
        panel_Notification.SetActive(false);
    }

    public void DisableUI()
    {
        panel_ChangeName.SetActive(false);
        panel_Menu.SetActive(false);
        panel_Options.SetActive(false);
        panel_HallOfDragons.SetActive(false);
        panel_RoomPanel.SetActive(false);
        panel_WalletConnecting.SetActive(false);

        //Disable dragons List
        //parent_Dragons.SetActive(false);
    }

    public void goto_HallOfDragons()
    {
        DisableUI();
        panel_HallOfDragons.SetActive(true);
        panel_HallOfDragons.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();
        parent_Dragons.SetActive(true);

        screen = MenuScreen.HallOfDragons;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);

        anim_MainCam.Play("mainCamBack", 0);
        anim_DragonCam.Play("DragonCamBack", 0);

        //dragonslist.SelectDragon(PlayerPrefs.GetInt("Selecteddragon"));
    }

    public void goto_ChangeName()
    {
        DisableUI();
        panel_ChangeName.SetActive(true);
        panel_ChangeName.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();

        screen = MenuScreen.ChangeName;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
    }

    public void goto_Menu()
    {
        DisableUI();
        panel_Menu.SetActive(true);
        panel_Menu.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();

        if (screen == MenuScreen.HallOfDragons)
        {
            anim_MainCam.Play("MainCam", 0);
            anim_DragonCam.Play("DragonCamForward", 0);
        }

        screen = MenuScreen.Menu;        

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
    }

    public void goto_Options()
    {
        DisableUI();
        panel_Options.SetActive(true);
        panel_Options.GetComponent<TweenCanvasGroupAlpha>().ResetAndPlay();

        screen = MenuScreen.Options;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
    }

    public void SetSoundButton()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            button_SoundOFF.gameObject.SetActive(true);
            button_SoundON.gameObject.SetActive(false);
            SoundManager.Instance.MuteSound();
        }
        else
        {
            button_SoundOFF.gameObject.SetActive(false);
            button_SoundON.gameObject.SetActive(true);
            SoundManager.Instance.UnMuteSound();
        }
    }

    public void button_Sound()
    {
        if(PlayerPrefs.GetInt("Sound") == 0)
        {
            button_SoundOFF.gameObject.SetActive(true);
            button_SoundON.gameObject.SetActive(false);
            SoundManager.Instance.MuteSound();

            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            button_SoundOFF.gameObject.SetActive(false);
            button_SoundON.gameObject.SetActive(true);
            SoundManager.Instance.UnMuteSound();

            PlayerPrefs.SetInt("Sound", 0);

            SoundManager.Instance.PlayOneShot(SoundManager.Instance.button_Menu);
        }
    }

    #endregion

    #region Room panel

    public void SelectMap(int map)
    {
        SelectedMap = map;
    }

    public void LoadGame()
    {
        if (!string.IsNullOrEmpty(input_RoomName.text) && (SelectedMap > 1) && APIManager.Instance.totalCoins >= CryptoManager.Instance.matchFees)
        {
            PlayerPrefs.SetString("RoomName", input_RoomName.text);
            //Destroy(NetworkRunnerMenu.gameObject);
            SceneManager.LoadScene(SelectedMap);           
        }        
    }

    public void SelectPlayerCount(int count)
    {
        PlayerPrefs.SetInt("PlayerCount", count);
        Debug.Log($"Room Player Count set to {PlayerPrefs.GetInt("PlayerCount")}");
    }

    #endregion

    #region Network
    public void EnablePlayButton()
    {
        image_Connecting.gameObject.SetActive(false);
        button_Play.gameObject.SetActive(true);
    }

    #endregion

    #region Hall of Dragons
    public void DeductCoins(int amount)
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - amount);
        CoinsMenu = PlayerPrefs.GetInt("Coins");
        APIManager.Instance.totalCoins = CoinsMenu;
    }

    #endregion

    #region WEB3
    public void DisableWEB3Panels()
    {
        panel_Approve.gameObject.SetActive(false);
        panel_BuyTokens.gameObject.SetActive(false);
    }

    public void Goto_ApprovePanel()
    {
        DisableWEB3Panels();
        panel_Approve.gameObject.SetActive(true);
    }

    public void Goto_BuyTokenPanel()
    {
        DisableWEB3Panels();
        panel_BuyTokens.SetActive(true);
    }

    public void Button_Approve()
    {
        CryptoManager.Instance.erC20Approve.Approve();
    }

    public void Button_BuyToken()
    {
        //if (string.IsNullOrEmpty(if_BuyToken.text)) return;

        BuyTokenAmount = float.Parse(if_BuyToken.text);
        CryptoManager.Instance.WEIConverter();
        CryptoManager.Instance.buyTokens.BuyToken();
    }

    public void Button_CloseBuyTokenPanel()
    {
        GameManager.instance.DisableWEB3Panels();        
    }

    #endregion
}


