using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgMusicManager : MonoBehaviour
{
    [SerializeField] Button MusicON;
    [SerializeField] Button MusicOFF;

    private bool Mute = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPress()
    {
        if(Mute == false)
        {
            AudioListener.pause = true;
            Mute = true;
        }
        else
        {
            AudioListener.pause = false;
            Mute = false;
        }
    }
}
