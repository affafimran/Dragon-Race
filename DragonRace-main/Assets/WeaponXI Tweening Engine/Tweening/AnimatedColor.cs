using UnityEngine;

/// <summary>
/// Makes it possible to animate a color of the widget.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedColor : MonoBehaviour
{
    public Color color = Color.white;

    SpriteRenderer mWidget;

    void OnEnable() { mWidget = GetComponent<SpriteRenderer>(); LateUpdate(); }
    void LateUpdate() { mWidget.color = color; }
}
