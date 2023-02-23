using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform t_followTarget;

    private float orignalSpeed;
    private float slowSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (t_followTarget == null) return;

        transform.position = t_followTarget.position;
        transform.rotation = t_followTarget.rotation;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hurdle")
        {
            other.GetComponent<Hurdle>().DestroyHurdle();
            StartCoroutine(SlowDown());
        }
    }



    IEnumerator SlowDown()
    {
        var spline = t_followTarget.GetComponent<SplineFollower>();
        orignalSpeed = spline.followSpeed;
        slowSpeed = orignalSpeed * 0.5f;

        spline.followSpeed = slowSpeed;
        yield return new WaitForSeconds(2f);

        spline.followSpeed = orignalSpeed;
    }
}
