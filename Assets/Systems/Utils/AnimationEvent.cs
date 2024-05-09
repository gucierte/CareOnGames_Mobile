using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEvent : MonoBehaviour
{
    [System.Serializable]
    public class action
    {
        public string name;
        public Button.ButtonClickedEvent Event;
    }
    [SerializeField] public List<action> Actions = new List<action>();

    public action GetActionByName(string search)
    {
        action r = null;

        foreach (var a in Actions)
        {
            if (a.name == search)
            {
                r = a;
                break;
            }
        }

        return r;
    }
    public void CallEvent(string eventName)
    {
        GetActionByName(eventName).Event.Invoke();
    }
}
