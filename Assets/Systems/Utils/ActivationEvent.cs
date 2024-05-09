using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivationEvent : MonoBehaviour
{
    public float Delay;
    public Toggle.ToggleEvent OnStartEvent;
    public Toggle.ToggleEvent OnEnableEvent;
    public Toggle.ToggleEvent OnDisableEvent;

    public void CallStartEvent()
    {
        OnStartEvent.Invoke(true);
    }
    public void CallEnableEvents()
    {
        OnEnableEvent.Invoke(true);
    }
    public void CallDisanableEvents()
    {
        OnDisableEvent.Invoke(false);
    }

    private void Start()
    {
        Invoke(nameof(CallStartEvent), Delay);
    }

    private void OnEnable()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Invoke(nameof(CallEnableEvents), Delay);
        }

    }

    private void OnDisable()
    {
        OnDisableEvent.Invoke(false);
    }
}
