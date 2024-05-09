using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitNinjaSlowmoSFX : settingsVolume
{
    private void LateUpdate()
    {
        if (Time.timeScale >= 1)
        {
            Source.volume = 0;
        } else
        {
            Source.volume = 1;
        }
        LimitVolume();
    }
}
