using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject dragon1;
    public GameObject text1;

    public GameObject dragon2;
    public GameObject text2;

    public GameObject dragon3;
    public GameObject text3;

    public GameObject dragon4;
    public GameObject text4;


    //public Text phyDef;
    //public Text phyAttack;
    //public Text mgclDef;
    //public Text mgclAttack;

    [SerializeField] Text phyDef, phyAttack, mgclDef, mgclAttack;

    DarkGreen DarkGreenDragon;
    BrownDragon BrownColorDragon;
    BlueDragon BlueColorDragon;
    PinkDragon PinkColorDragon;

    // Start is called before the first frame update
    void Start()
    {
        dragon1.SetActive(false);
        dragon2.SetActive(false);
        dragon3.SetActive(false);
        dragon4.SetActive(false);

        phyDef.text = "0";
        phyAttack.text = "0";
        mgclAttack.text = "0";
        mgclDef.text = "0";


        //accessing dragon1 script
        DarkGreenDragon= GetComponent<DarkGreen>();
        BlueColorDragon = GetComponent<BlueDragon>();
        BrownColorDragon= GetComponent<BrownDragon>();
        PinkColorDragon = GetComponent<PinkDragon>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    public void OnClickButton()
    {
        dragon1.SetActive(true);
        dragon2.SetActive(false);
        dragon3.SetActive(false);
        dragon4.SetActive(false);

        phyDef.text = BlueColorDragon.phyAttack.ToString(); //add all int to public 

    }

    public void OnClickButton2()
    {
        dragon1.SetActive(false);
        dragon2.SetActive(true);
        dragon3.SetActive(false);
        dragon4.SetActive(false);
    }

    public void OnClickButton3()
    {
        dragon1.SetActive(false);
        dragon2.SetActive(false);
        dragon3.SetActive(true);
        dragon4.SetActive(false);
    }

    public void OnClickButton4()
    {
        dragon1.SetActive(false);
        dragon2.SetActive(false);
        dragon3.SetActive(false);
        dragon4.SetActive(true);
    }














}
