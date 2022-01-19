using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool startOff;
    [SerializeField] private bool loop;
    public Material material;
    Light light;
    public float minOff;
    public float maxOff;
    public float lightOn;

    private void Awake()
    {
        light = GetComponentInChildren<Light>();

        if (startOff)
        {
            light.enabled = false;
            material.SetColor("_EmissionColor", Color.white * -100);
        }
    }

    void Start()
    {       
        if(playOnStart)
            StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        if(light.enabled == true)
        {
            light.enabled = false;
            material.SetColor("_EmissionColor", Color.white * -100);
            yield return new WaitForSeconds(Random.Range(minOff, maxOff));
        }
        if (light.enabled == false)
        {
            light.enabled = true;
            material.SetColor("_EmissionColor", Color.white * 100);
            yield return new WaitForSeconds(lightOn);
        }

        if(loop)
            StartCoroutine(UpdateTimer());
    }

    IEnumerator Flash()
    {
        light.enabled = true;
        material.SetColor("_EmissionColor", Color.white * 100);
        yield return new WaitForSeconds(lightOn);
        light.enabled = false;
        material.SetColor("_EmissionColor", Color.white * -100);
        yield break;
    }

    public void StartFlash()
    {
        StartCoroutine(Flash());
    }

    public void SetLoop(bool value)
    {
        loop = value;
    }
}
