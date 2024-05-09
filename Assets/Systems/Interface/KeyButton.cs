using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    public KeyCode Key;
    public Button target;

    private void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            if (target.isActiveAndEnabled && target.interactable)
            {
                target.OnSubmit(new BaseEventData(EventSystem.current));
            }
        }
    }
}
