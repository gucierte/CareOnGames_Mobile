using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class MessageSender
{
    [System.Serializable]
    public class Message
    {
        public string Invoke;
        public string Params;
        public GameObject target { get; set; }
        public void SendMessage()
        {
            target.SendMessage(Invoke, Params);
        }
    }

    public static bool Send(string Method, object paramter)
    {
        bool hasReciver = true;
        foreach (var item in MonoBehaviour.FindObjectsOfType<MonoBehaviour>())
        {
            item.BroadcastMessage(Method, paramter, SendMessageOptions.DontRequireReceiver);
        }

        return hasReciver;
    }

    public static bool Send(string Method)
    {
        bool hasReciver = true;
        foreach (var item in MonoBehaviour.FindObjectsOfType<MonoBehaviour>())
        {
            item.BroadcastMessage(Method, SendMessageOptions.DontRequireReceiver);
        }

        return hasReciver;
    }
}
