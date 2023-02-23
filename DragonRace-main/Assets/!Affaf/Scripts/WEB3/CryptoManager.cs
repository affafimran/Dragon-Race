using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptoManager : MonoBehaviour
{
    public static CryptoManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public ERC20Approve erC20Approve;
    public ERC20Allowance erC20Allowance;
    public BuyTokens buyTokens;
    public EthBalanceOfExample ethBalance;
    public StartNewGame StartNewGame;
    public ClaimReward claimReward;
    public NeedToBeClaimed NeedToBeClaimed;

    [Space]

    public int matchFees = 2830;

    #region Converters
    public void WEIConverter()
    {
        float eth = float.Parse($"{GameManager.instance.BuyTokenAmount}");
        float decimals = 1000000000000000000; // 18 decimals
        float wei = eth * decimals;

        GameManager.instance.buyTokenAmountInWEI = Convert.ToDecimal(wei).ToString();

        print("Value in WEI: " + Convert.ToDecimal(wei).ToString());
    }

    public void EthConverter(string response)
    {
        float wei = float.Parse(response);
        float decimals = 1000000000000000000; // 18 decimals
        float eth = wei / decimals;
        if(eth > APIManager.Instance.totalCoins && !GameManager.instance.panel_Approve.activeInHierarchy)
        {
            Debug.Log($"balance Update Successful");
            GameManager.instance.Button_CloseBuyTokenPanel();
        }
        APIManager.Instance.totalCoins = eth;
        print("Value in ETH: " + Convert.ToDecimal(eth).ToString());
    }
    #endregion

    #region Balance
    public void UpdateBalance()
    {
        InvokeRepeating("checkBalance", 1f, 5f);
    }

    void checkBalance()
    {
        ethBalance.BalanceOf();
    }
    #endregion
}
