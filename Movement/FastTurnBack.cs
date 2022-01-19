using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FastTurnBack : MonoBehaviour
{
    [SerializeField] private float turnBackTime = 0.1f;
    [SerializeField]private CinemachineRecomposer cinemachineRecomposer;
    private InputController inputController;

    private float angle = 0;

    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        inputController.OnInstantlyTurnBack += InputController_OnInstantlyTurnBack;
    }

    private void InputController_OnInstantlyTurnBack()
    {
        StopAllCoroutines();
        if (angle == 0)
        {
            angle = 180;

            StartCoroutine(TurnBack(0,angle));
        }
        else
        {
            angle = 0;

            StartCoroutine(TurnBack(180,angle));
        }
    }

   IEnumerator TurnBack(float currentAngle,float targetAngle)
   {
        float currentTime = 0;
        float value = 0;
        float addValue = 1 / turnBackTime;
        do
        {
            currentTime += Time.deltaTime;
            value += addValue * Time.deltaTime;

            cinemachineRecomposer.m_Pan = (targetAngle*value)-(currentAngle - (currentAngle * value));

            yield return null;

        } while (currentTime < turnBackTime);

        cinemachineRecomposer.m_Pan = targetAngle;
   }
}
