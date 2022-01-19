using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFadeOut : MonoBehaviour
{
    [SerializeField] private GeneralConfig generalConfig;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AnimationCurve fadeOutCurve;

    [Range(0,1)]
    [SerializeField] private float defaltVolume;

    private float maxVolume;

    private Coroutine fadeOutCoroutine;

    private void Awake()
    {
        UpdateVolume();

        generalConfig.OnValueModify += UpdateVolume;
    }

    public void PlayAudio()
    {
        if (audioSource.isPlaying == false)
        {
            audioSource.volume = maxVolume;
            audioSource.Play();
        }
        else
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
            }

            audioSource.volume = maxVolume;
        }
    }

    public void StopAudio()
    {
       fadeOutCoroutine = StartCoroutine(StopAudioWhitFadeOut());
    }

    IEnumerator StopAudioWhitFadeOut()
    {
        float currentTime = 0;

        float startVolume = audioSource.volume;

        float fadeOutTime = fadeOutCurve[fadeOutCurve.length -1].time;
        print(fadeOutTime);
        do
        {
            currentTime += Time.deltaTime;
          
            audioSource.volume = startVolume - (startVolume * fadeOutCurve.Evaluate(currentTime));

            yield return new WaitForEndOfFrame();

        } while (currentTime < fadeOutTime  || audioSource.isPlaying == false);

        audioSource.Stop();

        fadeOutCoroutine = null;
        yield break;
    }

    public void UpdateVolume()
    {
        float volumePercentage = generalConfig.gameVolume;

        volumePercentage = volumePercentage / 100;

        if (audioSource)
        {
            audioSource.volume = defaltVolume * volumePercentage;

            maxVolume = audioSource.volume;
        }
    }
}
