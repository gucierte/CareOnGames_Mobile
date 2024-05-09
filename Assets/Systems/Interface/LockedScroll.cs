using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedScroll : MonoBehaviour
{
    public Scrollbar Scrollbar;
    public int Steps;
    public int rawValue;
    public float value;


    private void Update()
    {
        int realSteps = Mathf.Clamp(Steps, 0, Steps - 1);
        if (Input.touchCount <= 0 && !Input.GetMouseButton(0))
        {
            float stepValue = (1f / realSteps) / 2;

            rawValue = (int)((Scrollbar.value + stepValue) * (realSteps));
            value = ((float)rawValue) / realSteps;
            Scrollbar.value = Mathf.Lerp(Scrollbar.value, value, 5 * Time.deltaTime);
        }
    }
}
