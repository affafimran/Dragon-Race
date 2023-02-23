using UnityEngine;

/// <summary>
/// Makes it possible to animate the widget's width and height using Unity's animations.
/// </summary>

[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
    public float width = 1f;
    public float height = 1f;

    SpriteRenderer mWidget;

    void OnEnable()
    {
        mWidget = GetComponent<SpriteRenderer>();
        LateUpdate();
    }

    void LateUpdate()
    {
        if (mWidget != null)
        {
            //mWidget.h = Mathf.RoundToInt(width);
            //mWidget.height = Mathf.RoundToInt(height);
        }
    }
}
