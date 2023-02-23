using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallDragonsManager : MonoBehaviour
{
    public static HallDragonsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }

    public int DragonToTrain;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TrainDragon(int dragon)
    {

    }
}
