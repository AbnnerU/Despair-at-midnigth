using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCicle : MonoBehaviour
{
    [SerializeField] private GeneralConfig generalConfig;

    [Range(0, 1)]
    [SerializeField] private float defaltVolume = 1;

    [SerializeField] private BreathSound breathSound;

    //[SerializeField] private BreathControl breathControl;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private BreathAudioConfig[] values;

    [Header("Use random clip")]

    [SerializeField] private RandomSound randomSound;

    private Coroutine audioClipCicleCoroutine=null;

    private float volume;

    private float customVolume;

    private float cicleTime;

    private int currentValueId = 0;

    private bool active = true;

    private bool breathControlActive = false;

    private void Awake()
    {
        UpdateVolume();

        audioSource.loop = false;

        breathSound.OnUpdateBreathValue += OnUpdateBreathValue;

        generalConfig.OnValueModify += UpdateVolume;

        //breathControl.OnStartBreathControl += BreathControl_OnStartBreathControl;
    }

    public void UpdateVolume()
    {
        float volumePercentage = generalConfig.gameVolume;

        volumePercentage = volumePercentage / 100;

        volume = defaltVolume * volumePercentage;
    }

    //private void BreathControl_OnStartBreathControl(bool active)
    //{
    //    breathControlActive = active;
    //}

    private void OnUpdateBreathValue(float currentBreathValue)
    {
        if(currentBreathValue > values[0].breathValue && audioClipCicleCoroutine == null)
        {
            cicleTime = values[0].soundCicleTime;

            audioSource.volume = volume * (values[0].volumePercentage / 100);

            currentValueId = 0;

            audioClipCicleCoroutine = StartCoroutine(AudioClipCicle());
        }
        else if(currentBreathValue < values[0].breathValue && audioClipCicleCoroutine != null)
        {
            StopCoroutine(audioClipCicleCoroutine);

            audioClipCicleCoroutine = null;
        }
        else if(currentBreathValue > values[0].breathValue)
        {           
            for(int i = 1; i < values.Length; i++)
            {
                if(currentBreathValue > values[i].breathValue)
                {
                    currentValueId = i;
                }
            }

            cicleTime = values[currentValueId].soundCicleTime;
            audioSource.volume = volume * (values[currentValueId].volumePercentage / 100);
        }
    }

    IEnumerator AudioClipCicle()
    {
        do
        {
            //if (breathControlActive )
            //{
            //    do
            //    {
            //        yield return null;

            //    } while (breathControlActive);
            //}
            if (randomSound != null)
            {
                audioSource.clip = randomSound.ChooseClip();
            }

            audioSource.Play();
            yield return null;
            
            while (audioSource.isPlaying == true)
            {
                if (active == false)
                {
                    audioSource.Stop();

                    audioClipCicleCoroutine = null;
                    yield break;
                }

                //if (breathControlActive)
                //{
                //    audioSource.Stop();
                //}


                yield return null;
                //cicleTime = values[currentValueId].soundCicleTime;
                //audioSource.volume = volume * (values[currentValueId].volumePercentage / 100);
            }

            yield return new WaitForSeconds(cicleTime);


        } while(active);

        audioClipCicleCoroutine = null;
        yield break;
        
    }
}

[System.Serializable]
public struct BreathAudioConfig
{
    public float breathValue;
    [Range(0,100)]
    public float volumePercentage;
    public float soundCicleTime;
}
