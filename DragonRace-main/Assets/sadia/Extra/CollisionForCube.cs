using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionForCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Cube collided");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Cube triggerd");
    }
}
