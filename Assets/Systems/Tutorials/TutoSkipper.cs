using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoSkipper : MonoBehaviour
{
    public Button.ButtonClickedEvent onSkip;

    void OnEnable()
    {
        if (SettingsMaster.main.SkipTutorials)
        {
            onSkip.Invoke();
        }
    }
}
