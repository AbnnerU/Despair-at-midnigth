using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundIntercaletion : AudioPlayer
{
    [SerializeField] private GeneralConfig generalConfig;

    [SerializeField] private bool useFadeControl;

    [SerializeField] private bool notRepeat=true;

    [Range(0,1)]
    [SerializeField] private float volume;

    [SerializeField] private SoundContol[] soundControl;

    [SerializeField] private AudioClip[] soundOptions;

    private int lastIndex=-1;

    public void PlayRandomSond()
    {
        int index = (int)Random.Range(0, soundOptions.Length);

        if (notRepeat)
        {
            if(index == lastIndex)
            {
                if (index!=soundOptions.Length -1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
        }

        for(int i = 0; i < soundControl.Length; i++)
        {
            AudioSource source = soundControl[i].audioSource;

            if (source.isPlaying == false)
            {
                if (useFadeControl)
                {
                    StartCoroutine(AudioVolumeControl(soundOptions[index], source, soundControl[i].curve, (volume*generalConfig.gameVolume)/100));
                }
                else
                {
                    source.clip = soundOptions[index];

                    if (source.clip != null)
                        OnPlayAudio?.Invoke(source.clip);

                    source.Play();
                }

                break;                
            }
        }
     

        lastIndex = index;
    }

    IEnumerator AudioVolumeControl(AudioClip clip,AudioSource audio,AnimationCurve curveRef, float targetVolume)
    {
        float clipLength = clip.length;

        float currentTime=0;

        float percentage = 0;

        audio.volume = 0;

        audio.clip = clip;

        if (audio.clip != null)
            OnPlayAudio?.Invoke(audio.clip);

        audio.Play();

        do
        {
            currentTime += Time.deltaTime;

            percentage = (currentTime * 100) / clipLength;

            audio.volume = targetVolume * curveRef.Evaluate(percentage / 100);

            yield return null;

        } while (currentTime < clipLength || audio.isPlaying==false);

        yield break;
    }


}

[System.Serializable]
public struct SoundContol
{
    public AudioSource audioSource;
    public AnimationCurve curve;
}
