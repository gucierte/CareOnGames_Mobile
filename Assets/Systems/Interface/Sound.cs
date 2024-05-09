using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static AudioSource Play(string AudioFilePath)
    {
        AudioClip clip = Resources.Load<AudioClip>(AudioFilePath);
        Debug.Log(clip);
        return Play(clip);
    }
    public static AudioSource Play(AudioClip clip)
    {
        AudioSource s = new GameObject(clip.name).AddComponent<AudioSource>();
        s.clip = clip;
        s.Play();
        s.volume = SettingsMaster.sfxVolume;
        Destroy(s.gameObject, clip.length);
        return s;
    }
}
