using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Custom Extention Methods as helpers.
/// </summary>
public static class ExtensionMethods
{

    public static void MakePixelPerfect(this SpriteRenderer _sp)
    {
        CustomSprite cache_sprite = _sp.GetComponent<CustomSprite>();
        if (cache_sprite != null && cache_sprite.enabled)
        {
            cache_sprite.height = _sp.sprite.bounds.size.y * 100;
            cache_sprite.width = _sp.sprite.bounds.size.x * 100;
            Vector2 val = new Vector2(cache_sprite.width, cache_sprite.height);
            cache_sprite.PixelPerfect();
        }
        else
        {
            Debug.LogError("No Custom Sprite Component Found..");
        }
    }
    public static void MakeCustomPixel(this SpriteRenderer _sp, float width, float height)
    {

        CustomSprite cache_sprite = _sp.GetComponent<CustomSprite>();
        if (cache_sprite != null && cache_sprite.enabled)
        {
            Vector2 val = new Vector2(width, height);
            cache_sprite.customPixelVector = val;
            cache_sprite.CustomPixel();
        }
        else
        {
            Debug.LogError("No Custom Sprite Component Found..");
        }

    }
    public static void ResetAndPlay(this UITweener uitweener)
    {
        uitweener.ResetToBeginning();
        uitweener.PlayForward();
    }



    /// <summary>
    /// Tranform Helpers
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    public static void XScaleSetter(this Transform transform, float x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public static void YScaleSetter(this Transform transform, float y)
    {
        transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
    }

    public static float XScaleGetter(this Transform transform)
    {
        return transform.localScale.x;
    }

    public static float YScaleGetter(this Transform transform)
    {
        return transform.localScale.y;
    }


    public static void ZoomIn(this Camera main, Vector3 position, float value)
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        foreach (Canvas temp in canvases)
        {
            temp.renderMode = RenderMode.WorldSpace;
        }
        CameraZoom cache = main.gameObject.GetComponent<CameraZoom>();
        if (cache == null)
        {
            main.gameObject.AddComponent<CameraZoom>();
            main.gameObject.GetComponent<CameraZoom>().toPosition = new Vector3(position.x, position.y, -10);
            main.gameObject.GetComponent<CameraZoom>().value = value;
            main.gameObject.GetComponent<CameraZoom>().ZoomIn();
        }
        else
        {
            cache.toPosition = new Vector3(position.x, position.y, -10);
            cache.value = value;
            cache.ZoomIn();
        }
    }

    public static void ZoomOut(this Camera main)
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        foreach (Canvas temp in canvases)
        {
            temp.renderMode = RenderMode.WorldSpace;
        }
        CameraZoom cache = main.gameObject.GetComponent<CameraZoom>();
        cache.ZoomOut();
    }

}
