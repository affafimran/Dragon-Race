using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// Custom minimalist Sprite Button works with both SpriteRenderer and UI Image
/// Offers Both Color Hovers and Sprite Hovers
/// </summary>

//[ExecuteInEditMode]
public class SpriteButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Color Hovers")]
    [Space(5)]
    public Color onPress = Color.white;
    public Color onRelease = Color.white;

    /// <summary>
    /// This is basic onPress Sprite hover..
    /// Add any other hovers if you want make sure 
    /// to implement perspective interfaces
    /// </summary>
    [Header("Sprite Hovers")]
    [Space(5)]
    public bool SpriteHovers;
    public Sprite onPressSprite;


    [Header("Hook Delegates")]
    [Space(8)]
    public UnityEvent onClick;     //UnityEvent if you have issues with EventDelegate



    private SpriteRenderer _spriteRenderer;
    private Image _image;
    private Sprite _normalSprite;
    private enum RendererMode
    {
        SpriteRenderer,
        Image,
    }
    private RendererMode _mode;
    void Start()
    {
        if (this.GetComponent<SpriteRenderer>() != null)
        {
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            _mode = RendererMode.SpriteRenderer;
            _normalSprite = _spriteRenderer.sprite;
        }
        else
        {
            _image = this.GetComponent<Image>();
            _mode = RendererMode.Image;
            _normalSprite = _image.sprite;
        }


    }

    public void OnPointerDown(PointerEventData eventData)
    {

        switch (_mode)
        {
            case RendererMode.SpriteRenderer:
                _spriteRenderer.color = onPress;
                //Debug.Log("Coming here");
                break;
            case RendererMode.Image:
                _image.color = onPress;
                break;
            default:
                Debug.LogWarning("No Sprite Renderer or UI Image Component Found");
                break;
        }
        if (SpriteHovers)
        {
            switch (_mode)
            {
                case RendererMode.SpriteRenderer:
                    _spriteRenderer.sprite = onPressSprite;
                    Debug.Log("Coming here");
                    break;
                case RendererMode.Image:
                    _image.sprite = onPressSprite;
                    break;
                default:
                    Debug.LogWarning("No Sprite Renderer or UI Image Component Found");
                    break;
            }

        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        switch (_mode)
        {
            case RendererMode.SpriteRenderer:
                _spriteRenderer.color = onRelease;
                break;
            case RendererMode.Image:
                _image.color = onRelease;
                break;
            default:
                Debug.LogWarning("No Sprite Renderer or UI Image Component Found");
                break;
        }

        if (SpriteHovers)
        {
            switch (_mode)
            {
                case RendererMode.SpriteRenderer:
                    _spriteRenderer.sprite = _normalSprite;
                    break;
                case RendererMode.Image:
                    _image.sprite = _normalSprite;
                    break;
                default:
                    Debug.LogWarning("No Sprite Renderer or UI Image Component Found");
                    break;
            }

        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();         //UnityEvent

    }

    public void TestClick()
    {
        Debug.Log(this.gameObject.name + " is Showing Click Debug");
    }
}
