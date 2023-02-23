

using UnityEngine;
using System.Collections;




public class UIDragObject : MonoBehaviour
{
    public enum ClampType
    {
        X,
        Y,
        XY,

    }

    public bool UseOffset;
    [HideInInspector]
    public Vector2 offset = Vector2.zero;       //= new Vector2(0f, 0.7f);

    public bool isClamped = false;
    [HideInInspector]
    public ClampType clampType;
    [HideInInspector]
    public float Min_X, Max_X;
    [HideInInspector]
    public float Min_Y, Max_Y;



    void OnMouseDrag()
    {
        if (this.GetComponent<Collider2D>().enabled)
        {
            Vector3 MovePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, this.transform.position.z);
            if (UseOffset)
                MovePos = MovePos + (new Vector3(offset.x, offset.y, this.transform.position.z));

            this.transform.position = MovePos;
        }
    }


    void Update()
    {
        if (isClamped)
        {
            switch (clampType)
            {
                case ClampType.X:
                    this.transform.localPosition = new Vector2(Mathf.Clamp(this.transform.localPosition.x, Min_X, Max_X),
                       this.transform.localPosition.y);
                    break;
                case ClampType.Y:
                    this.transform.localPosition = new Vector2(this.transform.localPosition.x,
                      Mathf.Clamp(this.transform.localPosition.y, Min_Y, Max_Y));
                    break;
                case ClampType.XY:
                    this.transform.localPosition = new Vector2(Mathf.Clamp(this.transform.localPosition.x, Min_X, Max_X),
                      Mathf.Clamp(this.transform.localPosition.y, Min_Y, Max_Y));
                    break;
            }
        }
    }

}
