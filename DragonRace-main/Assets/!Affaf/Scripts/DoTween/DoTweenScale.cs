using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoTweenScale : MonoBehaviour
{
    public LoopType loopType;
    [Tooltip("This defines start or end animations like bounce etc")]
    public Ease Ease;

    [Space]

    public int loops;
    public float duration;

    [Space]

    public Vector3 FromScale;
    public Vector3 ToScale;

    [Header("For Shake")]

    public bool Shake;

    public float strength = 1f;
    public int intensity = 1;
    public float randomness = 90;
    public bool fade = true;

    public ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;

    [Space]

    public UnityEvent OnFinish;

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        transform.localScale = FromScale;

        if (Shake)
            GetComponent<Transform>().DOShakeScale(duration, strength, intensity, randomness, fade, randomnessMode);
        else
            GetComponent<Transform>().DOScale(ToScale, duration).SetLoops(loops, loopType).SetEase(Ease).onComplete = Finish;
    }

    private void Finish()
    {
        OnFinish?.Invoke();
    }
}

