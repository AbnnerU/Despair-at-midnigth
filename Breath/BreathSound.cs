using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
public class BreathSound : SoundAreaCreator
{
    [Space(30)]

    //[SerializeField] private TMP_Text textComponent;

    [SerializeField] private float maxBreathValue;

    [SerializeField] private float executionInterval;

    [SerializeField] private float decreaseValue;

    //[SerializeField] private Transform soundStartPoint;

    [SerializeField] private BreathSoundConfig[] triggerValues;

    [SerializeField] protected bool drawGizmos;

    [Header("Running")]
    [SerializeField] private FPSMovement fPSMovement;

    [SerializeField] private float runningAddValue;

    [Header("Enemy close")]
    [SerializeField] private Transform player;

    [SerializeField] private Transform target;

    [SerializeField] private float minDistance;

    [SerializeField]private float enemyCloseAddValue;

    public Action<float> OnUpdateBreathValue;

    private float currentBreathValue;

    private float currentDistance;

    private bool running;

    private bool enemyClose=false;

    private bool active=true;

    private bool canMakeSound = true;

    private bool evalueBreath=true;

    protected override void Awake()
    {
        base.Awake();


        if (fPSMovement == null)
        {
            fPSMovement = FindObjectOfType<FPSMovement>();
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        fPSMovement.OnRun += FPSMovement_OnRun;


        StartCoroutine(BreathSoundCoroutine());
    }
    
    public void SetActive(bool value)
    {
        active = value;       
    }


    public void ResetBreathValue()
    {
        currentBreathValue = 0;
        //textComponent.text = currentBreathValue.ToString();

        OnUpdateBreathValue?.Invoke(currentBreathValue);
    }

    IEnumerator BreathSoundCoroutine()
    {
        do
        {
            if (evalueBreath)
            {
                enemyClose = false;

                currentDistance = (target.position - player.position).magnitude;

                if (target.gameObject.activeSelf==true && currentDistance < minDistance)
                {
                    currentBreathValue = Mathf.Clamp(currentBreathValue + (enemyCloseAddValue * Time.deltaTime), 0, maxBreathValue);
                    enemyClose = true;
                }



                if (running)
                {
                    currentBreathValue = Mathf.Clamp(currentBreathValue + (runningAddValue * Time.deltaTime), 0, maxBreathValue);

                }
                else if (enemyClose)
                {
                    currentBreathValue = Mathf.Clamp(currentBreathValue + (enemyCloseAddValue * Time.deltaTime), 0, maxBreathValue);
                }
                else
                {
                    currentBreathValue = Mathf.Clamp(currentBreathValue - (decreaseValue * Time.deltaTime), 0, maxBreathValue);
                }

                //print(currentBreathValue);
                if(running==false)
                   CreateAnSound();
            }

            OnUpdateBreathValue?.Invoke(currentBreathValue);

            //textComponent.text = currentBreathValue.ToString();

            yield return new WaitForSeconds(executionInterval);

        } while (active);

        yield break;
    }


    public void CreateAnSound()
    {
        
        float areaValue = 0;
        float intensityValue = 0;

        if (currentBreathValue < triggerValues[0].breathToActive)
            return;

        for(int i = 0; i < triggerValues.Length; i++)
        {
            if (currentBreathValue > triggerValues[i].breathToActive)
            {
                areaValue = triggerValues[i].soundArea;
                intensityValue = triggerValues[i].soundIntensity;
            }                
        }

        //print("SOUND VALUE: "+areaValue);
        StartCoroutine(NewSoundArea(soundConfig.soundTransform, areaValue, intensityValue));
    }


    public void SetEvalueBreath(bool value)
    {
        evalueBreath = value;
    }

    public float GetBreathMaxValue()
    {
        return maxBreathValue;
    }

    public float GetCurrentBreathValue()
    {
        return currentBreathValue;
    }


    private void FPSMovement_OnRun(bool running)
    {
        this.running = running;
    }

    public void AddBreathValue(float addValue)
    {
        currentBreathValue = Mathf.Clamp(currentBreathValue + addValue, 0, maxBreathValue);

        OnUpdateBreathValue?.Invoke(currentBreathValue);
        //print("RESPIRAÇÂO: " + currentBreathValue);
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {

            if (drawSound)
            {
                Gizmos.color = Color.blue;
                drawSound = false;

                Gizmos.DrawSphere(soundConfig.soundTransform.position, drawArea);
            }
        }
    }

}

[System.Serializable]
public struct BreathSoundConfig
{
    public float breathToActive;
    public float soundArea;
    public float soundIntensity;
}

