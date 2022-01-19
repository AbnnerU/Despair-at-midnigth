using System.Collections;
using UnityEngine;

public class RandomSound : AudioPlayer
{
    [SerializeField] private bool notRepeat = true;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] soundOptions;

    [SerializeField] private AudioClip specialAudio;

    [SerializeField] private int playInExecutuion=0;

    private int currentExecution = 0;

    public OnSoundAction OnEndClip;

    private bool usingAudioSource = false;

    private int lastIndex = -1;

    public void PlayRandomSond()
    {
        if (usingAudioSource == false)
        {
            currentExecution++;

            if (currentExecution == playInExecutuion)
            {
                audioSource.clip = specialAudio;
            }
            else
            {
                audioSource.clip = ChooseClip();
            }

            if (audioSource.clip != null)
                OnPlayAudio?.Invoke(audioSource.clip);


            audioSource.Play();

            StartCoroutine(VerifyAudioSource());
        }
    }

    IEnumerator VerifyAudioSource()
    {
        while (audioSource.isPlaying == true)
        {
            yield return null;
        }

        OnEndClip?.Invoke();

        usingAudioSource = false;

        yield break;
    }
    
    public AudioClip ChooseClip()
    {
        int index = (int)Random.Range(0, soundOptions.Length);

        if (notRepeat)
        {
            if (index == lastIndex)
            {
                if (index != soundOptions.Length - 1)
                {
                    index++;
                }
                else
                {
                    index = 0;
                }
            }
        }

        return soundOptions[index];
    }
}
