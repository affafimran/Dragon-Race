using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Text PlayerName;
    public GameObject WinnerMedal;
    public GameObject SilverMedal;
    public GameObject BronzeMedal;
    
    /// <summary>
    /// Fill Result UI fields
    /// </summary>
    /// <param name="result"></param>
    /// <param name="place"></param>
    public void SetResult(string result, int place)
    {
        PlayerName.text = $"{place}- {result}";
    }
}
