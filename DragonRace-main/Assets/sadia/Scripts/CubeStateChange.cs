using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeStateChange : MonoBehaviour
{
    public GameObject cube;
   public  GameObject cylinder;
    public static float time;
    public static float startTime = 5;
    public Text countdownText;

    void Start()
    {
        cube.SetActive(true);
        cylinder.SetActive(false);
       
        
        time = startTime;
        StartCoroutine(LoseTime());
        Time.timeScale = 1;
        countdownText.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        countdownText.text = time.ToString(); //Showing the Score on the Canvas
        print(time);

        if(time == 0)
        {
            cube.SetActive(false);
            cylinder.SetActive(true);
        }
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            time--;
        }
    }

    
}
