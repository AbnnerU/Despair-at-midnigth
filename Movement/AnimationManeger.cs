using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManeger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField]private FPSMovement fPSMovement;

    [SerializeField] private CrounchHoldButton crounch;
    [SerializeField] private Prone prone;

    private void Awake()
    {
        fPSMovement = GetComponent<FPSMovement>();

        fPSMovement.OnNewInput += FPSMovement_OnNewInput;
        fPSMovement.OnMove += FPSMovement_OnMove;
        fPSMovement.OnRun += FPSMovement_OnRun;

        fPSMovement.OnJumping += FPSMovement_OnJumping;


        if (crounch != null)
        {
            crounch.OnCrounch += Crounch_OnCrounch;
        }

        if (prone != null)
        {
            prone.OnCrounch += Prone_OnCrounch;
        }
    }

    #region Defalft Movement Animations
    private void FPSMovement_OnNewInput(Vector2 inputValue)
    {
        if (inputValue.y < 0)
        {
            anim.SetBool("MovingBack", true);
        }
        else
        {
            anim.SetBool("MovingBack", false);
        }
    }

    private void FPSMovement_OnMove(float movementValue)
    {
        //print(movementValue);
        anim.SetFloat("Movement", movementValue);
    }

    private void FPSMovement_OnRun(bool runnig)
    {
        anim.SetBool("Runnig", runnig);
    }

    private void FPSMovement_OnJumping(bool jumping)
    {
        anim.SetBool("Jumping", jumping);
    }
    #endregion

    private void Crounch_OnCrounch(bool crounched)
    {
        anim.SetBool("Crounch", crounched);
    }

    private void Prone_OnCrounch(bool prone)
    {
        anim.SetBool("Prone", prone);
    }


    private void OnDisable()
    {
        anim.SetBool("Crounch", false);
        anim.SetBool("Prone", false);
        anim.SetBool("MovingBack", false);
        anim.SetBool("Jumping", false);
        anim.SetFloat("Movement",0);
    }
}
