using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public string defaultURL;
    public void Open()
    {
        Open(defaultURL);
    }
    public void Open(string URL)
    {
        Application.OpenURL(URL);
    }

    public void OpenWithoutPopUp(string URL)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        Nfynt.NPlugin.OpenURLInSameTab(URL);
        #endif
    }
}
