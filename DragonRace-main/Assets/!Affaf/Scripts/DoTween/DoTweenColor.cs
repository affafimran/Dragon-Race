using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DoTweenColor : MonoBehaviour
{
    public LoopType loopType;
    public ColorObject colorObject;

    [Space]

    public int loops;
    public float duration;

    [Space]

    public Color FromColor;
    public Color ToColor;

    //public UnityEvent OnFinished;
    
    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {

        if(colorObject == ColorObject.Text)
        {
            var color = GetComponent<Text>();
            color.color = FromColor;
            color.DOColor(ToColor, duration).SetLoops(loops, loopType);
        }

        if (colorObject == ColorObject.Image)
        {
            var color = GetComponent<Image>();
            color.color = FromColor;
            color.DOColor(ToColor, duration).SetLoops(loops, loopType);
        }

        if (colorObject == ColorObject.Sprite)
        {
            var color = GetComponent<SpriteRenderer>();
            color.color = FromColor;
            color.DOColor(ToColor, duration).SetLoops(loops, loopType);
        }
    }

}

public enum ColorObject
{
    Text,
    Image,
    Sprite,
}
