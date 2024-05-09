using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionEvent : MonoBehaviour
{
    public bool PerformOnEnable;
    public static ScreenOrientation Orientation;
    public Toggle.ToggleEvent OnVertical;
    public Toggle.ToggleEvent OnHorizontal;

    private void OnEnable()
    {
        if (!PerformOnEnable)
            return;
        if (Screen.width > Screen.height)
        {
            OnHorizontal.Invoke(true);
        }
        else
        {
            OnVertical.Invoke(true);
        }
    }

    private void LateUpdate()
    {
        if(Screen.width > Screen.height)
        {
            if (Orientation != ScreenOrientation.LandscapeLeft)
            {
                OnHorizontal.Invoke(true);
                Orientation = ScreenOrientation.LandscapeLeft;
            }
        } else
        {
            if (Orientation != ScreenOrientation.Portrait)
            {
                OnVertical.Invoke(true);
                Orientation = ScreenOrientation.Portrait;
            }
        }
        Debug.Log(Orientation);
    }
}
