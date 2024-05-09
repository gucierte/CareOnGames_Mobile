using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsVolume : MonoBehaviour
{
    [System.Serializable]
    public enum sourceType
    {
        SFX, Music
    }
    [SerializeField]
    public sourceType SourceType;
    public AudioSource Source;

    public void LimitVolume()
    {
        switch (SourceType)
        {
            case sourceType.SFX:
                Source.volume = Mathf.Clamp(Source.volume, 0, SettingsMaster.sfxVolume);
                break;
            case sourceType.Music:
                Source.volume = Mathf.Clamp(Source.volume, 0, SettingsMaster.musicVolume);
                break;
            default:
                break;
        }
    }
    private void Awake()
    {
        LimitVolume();
    }
    private void Start()
    {
        LimitVolume();
    }

    private void OnValidate()
    {
        if (!Source)
        {
            Source = GetComponent<AudioSource>();
        }
    }
}
