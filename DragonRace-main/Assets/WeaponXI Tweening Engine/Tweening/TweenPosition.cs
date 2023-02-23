using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>

public class TweenPosition : UITweener
{
    [Space(15)]
    public Vector3 from;
    public Vector3 to;

    //[HideInInspector]
    public bool worldSpace = false;

    Transform mTrans;
    //UIRect mRect;

    public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

    [System.Obsolete("Use 'value' instead")]
    public Vector3 position { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public Vector3 value
    {
        get
        {
            return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
        }
        set
        {
            if (worldSpace)
            {
                if (worldSpace) cachedTransform.position = value;               // Stupidity
                else cachedTransform.localPosition = value;
            }
            else
            {
                cachedTransform.localPosition = value;                      // Customized 
                //value = cachedTransform.localPosition;
                //value -= cachedTransform.localPosition;
                //WeaponXIMath.MoveRect(mRect, value.x, value.y);
            }
        }
    }

    void Awake() { mTrans = GetComponent<Transform>(); }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenPosition Begin(GameObject go, float duration, Vector3 pos, bool worldSpace)
    {
        TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
        comp.worldSpace = worldSpace;
        comp.from = comp.value;
        comp.to = pos;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    [ContextMenu("Set 'From' to current value")]
    public override void SetStartToCurrentValue() { from = value; }

    [ContextMenu("Set 'To' to current value")]
    public override void SetEndToCurrentValue() { to = value; }

    [ContextMenu("Assume value of 'From'")]
    void SetCurrentValueToStart()
    {
        value = from;
        //cachedTransform.localPosition = from;                   // Customized 
    }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd()
    {
        value = to;
        //cachedTransform.localPosition = to;                     // Customized 
    }
}
