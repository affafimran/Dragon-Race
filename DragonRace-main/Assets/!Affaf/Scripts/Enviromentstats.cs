using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Enviromentstats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.Greenenvironment == true)
        {
            Invoke("setspeed", 1);
        }
        
    }
    public void setspeed()
    {
        Debug.Log("Aello");
        GetComponent<SplineFollower>().followSpeed = GetComponent<SplineFollower>().followSpeed  + 6;
    }
   
}
