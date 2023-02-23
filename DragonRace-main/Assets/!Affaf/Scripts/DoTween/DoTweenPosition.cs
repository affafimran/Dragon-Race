using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoTweenPosition : MonoBehaviour
{
    public LoopType loopType;
    [Tooltip("This defines start or end animations like bounce etc")]
    public Ease Ease = Ease.Linear;
    public bool isUI;

    [Space]

    public int loops;
    public float duration;
    public bool Snapping = true;

    [Space]

    public Vector3 From;
    public Vector3 To;

    [Header("For Shake")]

    public bool Jump;

    public float JumpPower = 1f;
    public int NumberOfJumps = 1;
    public float JumpDuration = 90;
    

    public ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Full;

    [Space]

    public UnityEvent OnFinish;

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (isUI)
        {
            Vector3 from = Camera.main.ScreenToWorldPoint(From);
            Vector3 to = Camera.main.ScreenToWorldPoint(To);

            transform.position = from;

            if (Jump)
                GetComponent<RectTransform>().DOJump(to, JumpPower, NumberOfJumps, JumpDuration, Snapping);
            else
                GetComponent<RectTransform>().DOMove(to, duration, Snapping).SetLoops(loops, loopType).SetEase(Ease).onComplete = Finish;
        }
        else
        {
            transform.position = From;

            if (Jump)
                GetComponent<Transform>().DOJump(To, JumpPower, NumberOfJumps, JumpDuration, Snapping);
            else
                GetComponent<Transform>().DOMove(To, duration, Snapping).SetLoops(loops, loopType).SetEase(Ease).onComplete = Finish;
        }
        

    }

    private void Finish()
    {
        OnFinish?.Invoke();
    }

}
