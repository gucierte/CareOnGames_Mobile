using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

public class InGameDebugger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public List<string> messages = new List<string>();


    private void Awake()
    {
        Application.logMessageReceivedThreaded += LogMessageUpdate;
    }
    private void LogMessageUpdate(string condition, string stackTrace, LogType type)
    {
        string message = $"<b>{condition}</b>\n<size=18>{stackTrace}</size>";
        string txt = "";
        switch (type)
        {
            case LogType.Error:
                txt = $"<b><color=red>[ERROR!]</b> {message} </color>";
                break;
            case LogType.Assert:
                txt = $"<b><color=white>[Assert]</b> {message} </color>";
                break;
            case LogType.Warning:
                txt = $"<b><color=orange>[Warning!]</b> {message} </color>";
                break;
            case LogType.Log:
                txt = $"<b><color=white>[Log]</b> {message} </color>";
                break;
            case LogType.Exception:
                txt = $"<b><color=red>[Exception]</b> {message} </color>";
                break;
            default:
                break;
        }

        if (!messages.Contains(txt))
        {
            messages.Add(txt);
        }

        text.text = "";

        foreach (var item in messages)
        {
            text.text += "\n" + item;
        }


    }

    private void OnValidate()
    {
        if (!text)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
    }
}
