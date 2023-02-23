using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenRectSize : UITweener
{
    [Space(15)]
    public Vector2 from = Vector2.zero;
    public Vector2 to = Vector2.zero;

    RectTransform mTrans;
    //UITable mTable;

    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = GetComponent<RectTransform>(); return mTrans; } }

    public Vector2 value { get { return cachedTransform.sizeDelta; } set { cachedTransform.sizeDelta = value; } }

    [System.Obsolete("Use 'value' instead")]
    public Vector2 scale { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from * (1f - factor) + to * factor;
    }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenRectSize Begin(GameObject go, float duration, Vector2 scale)
    {
        TweenRectSize comp = UITweener.Begin<TweenRectSize>(go, duration);
        comp.from = comp.value;
        comp.to = scale;

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
    void SetCurrentValueToStart() { value = from; }

    [ContextMenu("Assume value of 'To'")]
    void SetCurrentValueToEnd() { value = to; }
}

