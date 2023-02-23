using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class CustomSprite : MonoBehaviour
{

    public float width;
    public float height;
    public Vector2 customPixelVector;


    private float pixel_per_unit = 100;
    private SpriteRenderer _sp;
    private float s_width;
    private float s_height;

    void Start()
    {
        _sp = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        s_width = _sp.sprite.bounds.size.x * pixel_per_unit;
        s_height = _sp.sprite.bounds.size.y * pixel_per_unit;
    }

    public void PixelPerfect()
    {
        height = _sp.sprite.bounds.size.y * pixel_per_unit;
        width = _sp.sprite.bounds.size.x * pixel_per_unit;
        customPixelVector = new Vector2(width, height);
        transform.localScale = new Vector3(width / s_width, height / s_height, 1);
    }

    public void CustomPixel()
    {
        transform.localScale = new Vector3(customPixelVector.x / s_width, customPixelVector.y / s_height, 1);
    }

}
