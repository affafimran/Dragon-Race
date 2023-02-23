using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Dragonstats : MonoBehaviour
{
    public float Speed;
    public float Power;
    public string _Name;
   
    void Awake()
    {
        //GetComponentInParent<SplineFollower>().followSpeed = Speed;
       //GetStats();
    }

    public void SetStats(float speed, float power, string name)
    {
        Speed = speed;
        Power = power;
        //PlayerPrefs.SetFloat($"{name} Speed", Speed);
        //PlayerPrefs.SetFloat($"{name} Power", Power);
        Debug.Log($"Dragon speed {PlayerPrefs.GetFloat($"{_Name} Speed")} set for {_Name}");
    }
    
    public void GetStats()
    {
        for(int i = 0; i < Dragonslist.Instance.dragonStats.Length; i++)
        {
            if(Dragonslist.Instance.dragonStats[i].Key == _Name)
            {
                Speed = Dragonslist.Instance.dragonStats[i].Speed;
                Power = Dragonslist.Instance.dragonStats[i].Power;
                SetStats(PlayerPrefs.GetFloat($"{_Name} Speed"), PlayerPrefs.GetFloat($"{_Name} Power"), _Name);
                break;
            }
        }
    }
}
