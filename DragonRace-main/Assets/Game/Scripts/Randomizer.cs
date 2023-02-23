using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Randomizer : MonoBehaviour
{
    public DragonStat _defaultDragon;
    DragonStat _newDragon;
    // Start is called before the first frame update
    void Start()
    {
         _newDragon = new DragonStat(_defaultDragon);
        printData();
    }

    void printData()
    {
       

    }
}
