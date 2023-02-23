using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Playerpathing : MonoBehaviour
{
    public static Playerpathing instance;
    public GameObject[] Players;

    private void Awake()
    {
        instance = this;
    }

 
    // Start is called before the first frame update
    
   public void setplayer()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

}
