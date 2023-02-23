using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogs : MonoBehaviour
{
    public GameObject prefab_Message;
    public Transform transform_MessageContainer;

    private List<GameObject> gameLogs = new List<GameObject>();

    public void CreateMessage(string text)
    {
        var message = Instantiate(prefab_Message, transform_MessageContainer);
        message.GetComponent<Message>().UpdateText(text);
        gameLogs.Add(message);

        LimitMessages();
    }

    void LimitMessages()
    {
        if (gameLogs.Count > 5)
        {
            Destroy(gameLogs[0]);
            gameLogs.RemoveAt(0);
        }
    }
}
