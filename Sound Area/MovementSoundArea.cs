using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSoundArea : SoundAreaCreator
{
    [SerializeField] private Transform soundPoint;

    [SerializeField] protected bool drawGizmos;

    [SerializeField] private float soundInterval;

    [Header("Walk")]
    [SerializeField] private bool drawWalkArea;
    [SerializeField] private float walkSoundArea;
    [SerializeField] private float walkSoundItensity;


    [Header("Running")]
    [SerializeField] private bool drawRunningArea;
    [SerializeField] private float runningSoundArea;
    [SerializeField] private float runningSoundItensity;
  

    private FPSMovement fpsMovement;

    private CrounchHoldButton crounch;

    //private Prone prone;

    private bool active=true;

    private bool moving=false;

    private bool running=false;

 

    protected override void Awake()
    {
        base.Awake();

        fpsMovement = GetComponent<FPSMovement>();

        crounch = GetComponent<CrounchHoldButton>();

        //prone = GetComponent<Prone>();


        fpsMovement.OnRun += FPSMovement_OnRun;

        fpsMovement.OnMove += FPSMovement_OnMove;

        StartCoroutine(MovementSound());

    }

    private void FPSMovement_OnMove(float speed)
    {
     
        if (speed > 0.05f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

    }

    private void FPSMovement_OnRun(bool run)
    {
        running = run;
    }

    public void SetActive(bool value)
    {
        active = value;
    }

    public void StartMovementSound()
    {
        StartCoroutine(MovementSound());
    }
    
   IEnumerator MovementSound()
    {
        do
        {
            //print(moving);
            if (crounch.GetCrounching() /*|| prone.GetCrounching()*/)
            {
                //
            }           
            else if (moving && running)
            {
                CreateSoundArea(soundPoint, runningSoundArea, runningSoundItensity);
                yield return new WaitForSeconds(soundInterval);               
            }
            else if (moving)
            {
                CreateSoundArea(soundPoint, walkSoundArea, walkSoundItensity);
                yield return new WaitForSeconds(soundInterval);                
            }

            yield return null;
           
        } while (active);

        yield break;
    }


    public void CreateSoundArea(Transform startPosition, float area, float intensity)
    {
        StartCoroutine(NewSoundArea(startPosition, area, intensity));
    }

    public void CreateSoundArea(float area)
    {
        StartCoroutine(NewSoundArea(soundPoint, area, 20f));
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (soundPoint)
            {
                if (drawWalkArea)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(soundPoint.position, walkSoundArea);
                }

                if (drawRunningArea)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(soundPoint.position, runningSoundArea);
                }

            }

           
            if (drawSound)
            {
                Gizmos.color = Color.yellow;
                drawSound = false;

                Gizmos.DrawSphere(soundPoint.position, drawArea);
            }
        }
    }
}
