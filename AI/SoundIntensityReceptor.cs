using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIntensityReceptor : MonoBehaviour
{
    //[SerializeField] private SoundType priority=SoundType.PLAYER;

    [SerializeField] private float minimumSoundIntensity;

    [SerializeField] private float delayToStopHearSound;

    private SoundConfig currentSound;

    private Coroutine stopHearCorotine;

    private SoundType lastSoundType=SoundType.GENERIC;

    private Transform _transform;

    private bool heardSomething=false;

    private void Awake()
    {
        _transform = GetComponent<Transform>();

        currentSound = new SoundConfig();
        currentSound.soundType = SoundType.GENERIC;
    }


    public void RecibeSound(Transform origin, float soundIntensity, SoundType soundType)
    {

        //if (lastSoundType == priority && soundType != priority)
        //    return;

        if (currentSound.soundTransform != null)
        {
            if (currentSound.soundTransform == origin)
            {
                SetSoundConfig(origin, soundIntensity, soundType);

                return;
            }

            if ((origin.position - _transform.position).magnitude > (currentSound.soundTransform.position - _transform.position).magnitude)
                return;
        }

        if (soundIntensity > minimumSoundIntensity)
        {
            SetSoundConfig(origin, soundIntensity, soundType);
        }

    }

    private void SetSoundConfig(Transform origin, float soundIntensity, SoundType soundType)
    {
        if (stopHearCorotine != null)
            StopCoroutine(stopHearCorotine);

        heardSomething = true;

        lastSoundType = currentSound.soundType;

        currentSound.soundTransform = origin;

        currentSound.soundIntensity = soundIntensity;

        currentSound.soundType = soundType;

        stopHearCorotine = StartCoroutine(StopHear());
    }

    IEnumerator StopHear()
    {
        yield return new WaitForSeconds(delayToStopHearSound);
        heardSomething = false;
        lastSoundType = SoundType.GENERIC;
        currentSound.soundType = SoundType.GENERIC;

        yield break;
    }

    public void SetHeardSomething(bool value)
    {
        heardSomething = value;

        if (value == false)
        {
            if (stopHearCorotine != null)
                StopCoroutine(stopHearCorotine);
        }
    }

    public void SetLastSoundType(SoundType type)
    {
        lastSoundType = type;
    }

    public bool GetHeardSomething()
    {
        return heardSomething;
    }

    public Transform GetSoundOrigin()
    {
        return currentSound.soundTransform;
    }

    public SoundType GetCurrentSoundType()
    {
        return currentSound.soundType;
    }
    

    private void OnDrawGizmos()
    {
        if (heardSomething == false)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.up*1, 0.5f);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + transform.up * 1, 0.5f);
        }
    }
}
