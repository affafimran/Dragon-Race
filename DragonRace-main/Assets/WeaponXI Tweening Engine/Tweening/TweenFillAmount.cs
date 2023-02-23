using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenFillAmount : UITweener
{
    public float from = 1f;
    public float to = 1f;

    Image image;

    /// <summary>
    /// Camera that's being tweened.
    /// </summary>

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
	public Camera cachedCamera { get { if (mCam == null) mCam = camera; return mCam; } }
#else
    public Image cachedImage { get { if (image == null) image = GetComponent<Image>(); return image; } }
#endif

    [System.Obsolete("Use 'value' instead")]
    public float orthoSize { get { return this.value; } set { this.value = value; } }

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get { return cachedImage.fillAmount; }
        set { cachedImage.fillAmount = value; }
    }

    /// <summary>
    /// Tween the value.
    /// </summary>

    protected override void OnUpdate(float factor, bool isFinished) { value = from * (1f - factor) + to * factor; }

    /// <summary>
    /// Start the tweening operation.
    /// </summary>

    static public TweenFillAmount Begin(GameObject go, float duration, float to)
    {
        TweenFillAmount comp = UITweener.Begin<TweenFillAmount>(go, duration);
        comp.from = comp.value;
        comp.to = to;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }
        return comp;
    }

    public override void SetStartToCurrentValue() { from = value; }
    public override void SetEndToCurrentValue() { to = value; }
}
