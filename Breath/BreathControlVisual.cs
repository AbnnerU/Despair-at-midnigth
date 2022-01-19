using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class BreathControlVisual : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private BreathControl breathControl;

    [SerializeField] private float defaltFOV;

    [SerializeField] private float effectFinalFOV;

    [SerializeField] private float effectDuration;

    [SerializeField] private float timeToBacToDefalt;

    private Coroutine breathControlEffect=null;

    private Coroutine backToDefalt = null;

    private void Awake()
    {
        breathControl.OnStartBreathControl += BreathControl_OnStartBreathControl;
    }

    private void BreathControl_OnStartBreathControl(bool active)
    {
        //print("ACTYIVE: " + active);
        if (active)
        {
            if (breathControlEffect == null)
            {
                breathControlEffect = StartCoroutine(FOVEffect());

                if (backToDefalt != null)
                {
                    StopCoroutine(backToDefalt);

                    backToDefalt = null;
                }
            }
        }
        else
        {
            if (breathControlEffect != null)
            {
                StopCoroutine(breathControlEffect);

                breathControlEffect = null;

                if(backToDefalt==null)
                backToDefalt = StartCoroutine(BackFovToDefalt());
                //StopAllCoroutines();
            }

            virtualCamera.m_Lens.FieldOfView = defaltFOV;
        }
    }


    IEnumerator FOVEffect()
    {
        float currentValue = defaltFOV;

        float addValue = (effectFinalFOV - defaltFOV) / effectDuration;

        do
        {
            currentValue += addValue * Time.deltaTime;
            virtualCamera.m_Lens.FieldOfView = currentValue;

            yield return new WaitForEndOfFrame();

        } while (currentValue < effectFinalFOV);

        virtualCamera.m_Lens.FieldOfView = effectFinalFOV;

        yield break;
    }

    IEnumerator BackFovToDefalt()
    {
        float currentValue = virtualCamera.m_Lens.FieldOfView;

        float addValue = (currentValue - defaltFOV) / timeToBacToDefalt;

        do
        {
            currentValue -= addValue * Time.deltaTime;
            virtualCamera.m_Lens.FieldOfView = currentValue;

            yield return new WaitForEndOfFrame();

        } while (currentValue > defaltFOV);

        virtualCamera.m_Lens.FieldOfView = defaltFOV;

        yield break;

    }
}
