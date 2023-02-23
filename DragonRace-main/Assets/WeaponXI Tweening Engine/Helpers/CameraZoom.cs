using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public Vector3 toPosition;
    public float value;

    public Vector3 initialPos;
    public float initialvalue;

    private bool doZoomIn = false;


    private float t = 0;
    private float pos_t = 0;
    private Camera cache_camera;
    private float init_ortho;
    Vector3 lastPosition;
    void Start()
    {
        cache_camera = this.GetComponent<Camera>();
        initialvalue = cache_camera.orthographicSize;
        initialPos = transform.position;
        init_ortho = initialvalue;
    }

    void Update()
    {

        if (doZoomIn)
        {
            if (this.transform.position != toPosition)
            {

                //this.transform.position = Vector3.Lerp(lastPosition, toPosition, pos_t);
                this.transform.position = new Vector3(Mathf.Lerp(lastPosition.x, toPosition.x, pos_t), Mathf.Lerp(lastPosition.y, toPosition.y, pos_t), -10f);
                cache_camera.orthographicSize = Mathf.Lerp(init_ortho, value, t);
                t += 0.04f * Time.deltaTime * 20;
                pos_t += 0.04f * Time.deltaTime * 20;
                Debug.Log("++++++++++");
            }
            else
            {
                Debug.Log("***********");
                lastPosition = new Vector3(transform.position.x, transform.position.y, -10);
                init_ortho = cache_camera.orthographicSize;
                t = 0;
                pos_t = 0;
            }
        }
        else
        {
            //t = 0;
            //doZoomIn = false;
            if (this.transform.position != initialPos)
            {
                //this.transform.position = Vector3.Lerp(lastPosition, initialPos, pos_t);
                this.transform.position = new Vector3(Mathf.Lerp(lastPosition.x, initialPos.x, pos_t), Mathf.Lerp(lastPosition.y, initialPos.y, pos_t), -10f);
                cache_camera.orthographicSize = Mathf.Lerp(init_ortho, initialvalue, t);
                t += 0.04f * Time.deltaTime * 20;
                pos_t += 0.04f * Time.deltaTime * 20;
                Debug.Log("++++++++++");
            }
            else
            {
                Debug.Log("***********");
                lastPosition = new Vector3(transform.position.x, transform.position.y, -10);
                init_ortho = cache_camera.orthographicSize;
                t = 0;
                pos_t = 0;
                Destroy(this);
            }
        }

        //this.transform.position = new Vector3(wrapperPosition.x, wrapperPosition.y, -10);
    }

    public void ZoomIn()
    {
        t = 0;
        doZoomIn = true;

    }
    public void ZoomOut()
    {
        t = 0;
        doZoomIn = false;
    }
}
