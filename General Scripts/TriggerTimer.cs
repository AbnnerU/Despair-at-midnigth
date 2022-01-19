using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnTimerAction : UnityEvent { }

public class TriggerTimer : MonoBehaviour
{
    [SerializeField] private float minTimer;

    [SerializeField] private float maxTimer;

    [SerializeField] private bool playOnAwake;

    [SerializeField] private bool loop = true;

    public OnTimerAction OnTimerEndActions;

    public Action OnTimerEnd;  

    private void Awake()
    {
        if (playOnAwake)
            StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        StopCoroutine(Timer());

        StopAllCoroutines();
    }

    public void SetLoop(bool value)
    {
        loop = value;
    }
    
    IEnumerator Timer()
    {
        float chooseTimer = UnityEngine.Random.Range(minTimer, maxTimer);

        yield return new WaitForSeconds(chooseTimer);

        OnTimerEnd?.Invoke();

        OnTimerEndActions?.Invoke();

        if (loop)
            StartTimer();

        yield break;
    }

  public void CallEvent()
    {
        OnTimerEndActions?.Invoke();
    }
}
