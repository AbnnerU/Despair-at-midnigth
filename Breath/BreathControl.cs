using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BreathControl : MonoBehaviour
{
    [SerializeField] private BreathSound breathSound;

    [SerializeField] private FPSMovement fPSMovement;

    [SerializeField] private GameObject breathControlPanel;

    [SerializeField] private Slider slider;

    [SerializeField] private float barStartValue;

    [SerializeField] private float delayToStart;

    [SerializeField] private float whenFailRechargeTime;

    [SerializeField] private float whenSuccessRechargeTime;

    [Header("Start Breath Control")]

    [SerializeField] private float minBreathValueToActive;

    [SerializeField] private float startDecreaseBarSpeed;

    [Header("Safe zone interval")]
    [SerializeField] private float safeZoneMin;

    [SerializeField] private float safeZoneMax;

    [Header("Bar speed")]

    [SerializeField] private float maxDecreasingBarSpeed;

    [SerializeField] private float minDecreasingBarSpeed;

    [Header("Input")]

    [SerializeField] private float addValueOnInput;

    [SerializeField] private float onExitSafeZoneValue;

    [Header("Descrease breath value")]

    [SerializeField] private float breathDecreaseValue;

    public Action<bool> OnStartBreathControl;

    private BreathControlState controlState=BreathControlState.NOTRUNNING;

    private Coroutine barControlCoroutine=null;

    private InputController inputController;

    private float maxBreatheValue;

    private float decreaseBarSpeedDifference;

    private bool active = true;

    private bool breacthControlEnabled = false;

    private bool moving = false;

    private void Awake()
    {
        if (fPSMovement == null)
            fPSMovement = FindObjectOfType<FPSMovement>();

        inputController = FindObjectOfType<InputController>();

        maxBreatheValue = breathSound.GetBreathMaxValue();

        decreaseBarSpeedDifference = maxDecreasingBarSpeed - minDecreasingBarSpeed;

        inputController.OnControlBreath += AddBarValue;

        fPSMovement.OnMove += FPSMovement_OnMove;

        breathControlPanel.SetActive(false);
    }

   

    IEnumerator DecreaseBarValue(float startValue)
    {
        yield return new WaitForSeconds(delayToStart);

        breathControlPanel.SetActive(true);      

        float breathValuePercentage;

        float currentDecreaseSpeed;

        breathValuePercentage = ((breathSound.GetCurrentBreathValue() * 100) / maxBreatheValue) / 100;
        currentDecreaseSpeed = minDecreasingBarSpeed + (decreaseBarSpeedDifference * breathValuePercentage);

        slider.value = startValue;

        do
        {
            if(controlState == BreathControlState.WAITING)
            {
                slider.value -= startDecreaseBarSpeed * Time.deltaTime;             

            }
            else if (controlState == BreathControlState.RUNNIG)
            {
                breathSound.SetEvalueBreath(false);

                if (breathSound.GetCurrentBreathValue() == 0)
                {
                    OnStartBreathControl?.Invoke(false);

                    breathControlPanel.SetActive(false);

                    controlState = BreathControlState.SUCESS;

                    yield return new WaitForSeconds(whenSuccessRechargeTime);

                    breathSound.SetEvalueBreath(true);

                    controlState = BreathControlState.NOTRUNNING;

                    yield break;
                }

                slider.value -= currentDecreaseSpeed * Time.deltaTime;

                breathValuePercentage = ((breathSound.GetCurrentBreathValue() * 100) / maxBreatheValue) / 100;
                currentDecreaseSpeed = minDecreasingBarSpeed + (decreaseBarSpeedDifference * breathValuePercentage);            
            }

            yield return new WaitForFixedUpdate();

        } while (slider.value > safeZoneMin && slider.value < safeZoneMax);

        OnStartBreathControl?.Invoke(false);

        breathControlPanel.SetActive(false); 

        breathSound.AddBreathValue(onExitSafeZoneValue);

        breathSound.CreateAnSound();

        breathSound.SetEvalueBreath(true);

        controlState = BreathControlState.FAIL;

        yield return new WaitForSeconds(whenFailRechargeTime);

        controlState = BreathControlState.NOTRUNNING;     
            
        yield break;
    }

    public void AddBarValue()
    {
        if (active)
        {
            if (controlState == BreathControlState.WAITING)
            {
                controlState = BreathControlState.RUNNIG;

                breathSound.AddBreathValue(-breathDecreaseValue);
            }
            else if (controlState == BreathControlState.RUNNIG)
            {
                OnStartBreathControl?.Invoke(true);

                slider.value += addValueOnInput;

                breathSound.AddBreathValue(-breathDecreaseValue);
            }
        }
    }

    private void FPSMovement_OnMove(float moveSpeed)
    {
        if (active)
        {
            if (moveSpeed < 0.05f)
            {
                moving = false;

                if (controlState == BreathControlState.NOTRUNNING)
                {
                    if (breathSound.GetCurrentBreathValue() > minBreathValueToActive)
                    {
                        controlState = BreathControlState.WAITING;

                        barControlCoroutine = StartCoroutine(DecreaseBarValue(barStartValue));
                    }
                }
            }
            else
            {
                moving = true;

                if (controlState != BreathControlState.NOTRUNNING && controlState != BreathControlState.SUCESS)
                {
                    OnStartBreathControl?.Invoke(false);

                    breathControlPanel.SetActive(false);

                    breathSound.SetEvalueBreath(true);

                    controlState = BreathControlState.NOTRUNNING;

                    StopCoroutine(barControlCoroutine);
                }
            }
        }
    }

    public void StopBreathControl()
    {
        active = false;

        OnStartBreathControl?.Invoke(false);

        breathControlPanel.SetActive(false);

        breathSound.SetEvalueBreath(true);

        controlState = BreathControlState.NOTRUNNING;

        StopCoroutine(barControlCoroutine);
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public enum BreathControlState
    {
        NOTRUNNING,
        WAITING,
        RUNNIG,
        SUCESS,
        FAIL       
    }
}
