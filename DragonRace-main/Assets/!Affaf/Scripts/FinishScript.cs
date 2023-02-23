using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.Announcertext.enabled = true;
        GameManager.instance.Announcertext.text = other.gameObject.name +" Finished First ";
        
    }
}
