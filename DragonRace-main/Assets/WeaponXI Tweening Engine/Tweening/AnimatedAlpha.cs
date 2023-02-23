
using UnityEngine;

/// <summary>
/// Makes it possible to animate alpha of the widget or a panel.
/// </summary>

[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alpha = 1f;

    //UIWidget mWidget;
    //UIPanel mPanel;

    SpriteRenderer mWidget;

    void OnEnable()
    {

        mWidget = GetComponent<SpriteRenderer>();
        //mPanel = GetComponent<UIPanel>();
        LateUpdate();
    }

    void LateUpdate()
    {
        if (mWidget != null) mWidget.color = new Color(mWidget.color.r, mWidget.color.g, mWidget.color.b, alpha);
        //if (mPanel != null) mPanel.alpha = alpha;
    }
}
