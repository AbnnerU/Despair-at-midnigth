using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Legenda : MonoBehaviour
{
    [SerializeField] private AudioPlayer[] audioPlayer;

    [SerializeField] private List<SoundLegend> soundLegends;

    [SerializeField] private TMP_Text textComponent;

    private float clipDurationReference;

    public void Awake()
    {
        foreach(AudioPlayer ap in audioPlayer)
        {
            ap.OnPlayAudio += OnPlayNewAudio;

        }
    }

    private void OnPlayNewAudio(AudioClip clip)
    {
        int index = soundLegends.FindIndex(x => x.clipReference == clip);

        if (index >= 0)
        {
            StopCoroutine(LegendTime());

            textComponent.text = soundLegends[index].clipLegend;

            StartCoroutine(LegendTime());

            clipDurationReference = clip.length;
        }
    }


    IEnumerator LegendTime()
    {
        float currentTime = 0;
        do
        {
            currentTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();


        } while (currentTime < clipDurationReference);

        textComponent.text = "";

        yield break;
    }
}

[System.Serializable]
public struct SoundLegend
{
    public AudioClip clipReference;
    [TextArea]
    public string clipLegend;
}