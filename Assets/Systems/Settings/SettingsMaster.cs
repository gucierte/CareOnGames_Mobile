using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMaster : MonoBehaviour
{
    static SettingsMaster instance { get; set; }
    public static SettingsMaster main
    {
        get
        {
            if (!instance)
            {
                instance = FindFirstObjectByType<SettingsMaster>();
            } 
            return instance;
        }
    }

    public static float sfxVolume = 1.0f;
    public static float musicVolume = 1.0f;

    public Slider sfxSlider;
    public Slider musicSlider;

    public bool SkipTutorials { get; set; }

    public void UpdateSliders()
    {
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;
    }

    public void UpdateVolume()
    {
        sfxVolume = sfxSlider.value;
        musicVolume = musicSlider.value;
    }

    private void OnEnable()
    {
        UpdateSliders();
        gamePaused = true;
    }

    private void OnDisable()
    {
        gamePaused = false;
    }

    private void LateUpdate()
    {
        UpdateVolume();
    }

    public static bool gamePaused;

    public void Update()
    {
        if (gamePaused)
        {
            //Time.timeScale = 0;
        } else
        {
            //Time.timeScale = 1;
        }
    }
}
