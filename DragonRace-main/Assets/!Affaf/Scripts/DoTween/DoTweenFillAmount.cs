using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DoTweenFillAmount : MonoBehaviour
{
    private Image Image;

    public Ease Ease = Ease.Linear;

    [Range(0f, 1f)] public float From;
    [Range(0f, 1f)] public float To;

    [Space]

    public float Duration;

    [Space]

    public UnityEvent OnFinish;

    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        Play();
    }

    public void Play()
    {
        DOTween.To(() => Image.fillAmount, x => Image.fillAmount = x, To, Duration).SetEase(Ease).onComplete = Finish;
    }

    private void Finish()
    {
        OnFinish?.Invoke();
    }
}
