using UnityEngine;
using UnityEngine.EventSystems;

public class ToolController : MonoBehaviour
{


    private Vector3 startPos;
    private bool BounceBack = false;
    public bool Delayed_Position = false;
    [HideInInspector]
    public float Delay_Timer;



    void Start()
    {
        GetComponent<EventTrigger>();
        if (!Delayed_Position)
        {
            BounceBack = true;
            SetStartPosition();
        }
        else
        {
            BounceBack = false;
            Invoke("SetStartPosition", Delay_Timer);
        }
    }
    void Update()
    {
        if (BounceBack)
        {
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, startPos.x, 0.08f), Mathf.Lerp(transform.localPosition.y, startPos.y, 0.08f), transform.localPosition.z);
        }
    }

    void SetStartPosition()
    {
        BounceBack = true;
        startPos = this.transform.localPosition;
    }

}
