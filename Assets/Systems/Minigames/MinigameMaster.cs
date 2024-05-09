using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameMaster : MonoBehaviour
{
    public AudioClip MusicLow;
    public AudioClip MusicHigh;

    public virtual void Play()
    {
        if (MusicLow && MusicHigh)
        {
            MusicFlow.main.SetMusic(MusicHigh, MusicLow, .5f);
        }
    }

    public virtual void Stop()
    {
        MusicFlow.main.ResetMusic();
    }

    public virtual void Restart()
    {

    }
}
