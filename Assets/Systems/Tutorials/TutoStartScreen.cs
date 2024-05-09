using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoStartScreen : GuideTutorial
{
    public LockedScroll LockedScroll;
    float LastValue;
    public Button.ButtonClickedEvent OnFinishEvent;

    private void Awake()
    {
        LastValue = LockedScroll.value;
    }

    public void LateUpdate()
    {
        if (Input.touchCount <= 0 && !Input.GetMouseButton(0))
        {
            if (LastValue != LockedScroll.value)
            {
                OnFinishEvent.Invoke();
            }
            LastValue = LockedScroll.value;
        }

        if (ResolutionEvent.Orientation != ScreenOrientation.Portrait)
        {
            OnFinishEvent.Invoke();
        }
    }
}
