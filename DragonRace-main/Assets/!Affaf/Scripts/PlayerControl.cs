using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.UI;
using System;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{

    public GameObject button;

    private float orignalSpeed;
    private float slowSpeed;

   

    private void Start()
    {
        
    }

   

    public void Setsplines()
    {
        CharacterInputHandler.instance.isreadytogo = true;
        //my code-------------------------------------
        //CharacterInputHandler.instance.GetNetworkInput();
        //--------------------------------------------

        button.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Hurdle")
    //    {
    //        other.GetComponent<Hurdle>().DestroyHurdle();
    //        StartCoroutine(SlowDown());
    //    }
    //}

    

    //IEnumerator SlowDown()
    //{
    //    var spline = GetComponent<SplineFollower>();
    //    orignalSpeed = spline.followSpeed;
    //    slowSpeed = orignalSpeed * 0.5f;

    //    spline.followSpeed = slowSpeed;
    //    yield return new WaitForSeconds(2f);

    //    spline.followSpeed = orignalSpeed;
    //}
}
