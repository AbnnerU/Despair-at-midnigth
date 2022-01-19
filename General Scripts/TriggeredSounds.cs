using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnSoundAction : UnityEvent { }

public class TriggeredSounds : AudioPlayer
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClips;

    public OnSoundAction OnEndClip;

    private bool usingAudioSource=false;

    public void PlayAudio(int index)
    {
        if (usingAudioSource == false)
        {
            if (index > audioClips.Length - 1 || index < 0)
            {
                Debug.LogError("Index fora do range: " + index);
                return;
            }

            usingAudioSource = true;

            audioSource.clip = audioClips[index];

            OnPlayAudio?.Invoke(audioClips[index]);

            audioSource.Play();

            StartCoroutine(VerifyAudioSource());
        }
    }


    public void PlayAudioSource()
    {
        if (audioSource.clip != null)
            OnPlayAudio?.Invoke(audioSource.clip);

        audioSource.Play();
    }

    IEnumerator VerifyAudioSource()
    {
        while (audioSource.isPlaying==true)
        {
            yield return null;
        }

        OnEndClip?.Invoke();

        usingAudioSource = false;

        yield break;
    }
}
