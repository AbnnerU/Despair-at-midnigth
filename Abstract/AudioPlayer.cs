using System;
using UnityEngine;

public abstract class AudioPlayer : MonoBehaviour
{
    //[SerializeField] protected AudioSource source;

    public Action OnPlayingAudio;
    public Action<AudioClip> OnPlayAudio;

    //public virtual AudioSource GetAudioSource()
    //{
    //    return source;
    //}
}
