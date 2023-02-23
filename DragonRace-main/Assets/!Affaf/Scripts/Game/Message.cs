using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text text_Message;
    public float showDuration = 3f;

    public void UpdateText(string text)
    {
        text_Message.text = text;
        Invoke("Destroy", showDuration);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
