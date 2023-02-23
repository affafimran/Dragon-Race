using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnteredPlayer : MonoBehaviour
{
    public Text PlayerName;
    public GameObject ReadyMark;

    private string pName;

    /// <summary>
    /// Fill Result UI fields
    /// </summary>
    /// <param name="result"></param>
    /// <param name="place"></param>
    public void SetPlayer(string name, int place)
    {
        PlayerName.text = $"{place}- {name}";
        pName = name;
    }

    public void UpdateRanking(int place)
    {
        PlayerName.text = $"{place}- {pName}";
    }
}
