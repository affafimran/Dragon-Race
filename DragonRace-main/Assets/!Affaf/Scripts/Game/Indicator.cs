using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Transform t_Follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (t_Follow == null) return;
        Follow();
    }

    void Follow()
    {
        transform.position = t_Follow.position + (Vector3.up * 5);
    }
}
