using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prone : CrounchCharacter
{
    [Header("If as crounch")]

    [SerializeField] private Crounch crounchScript;

    protected override void Awake()
    {
        base.Awake();       

        inputController.OnProne += InputController_OnCrounch;

    }


    protected override void InputController_OnCrounch()
    {
        if (rayCheck.SphereCheck(groundCheckPosition.position,0,layerMask)==true )
        {
            if (crounching == false)
            {
                crounching = true;

                characterController.height = crounchHeight;

                characterController.center = crounchCenter;

                characterController.radius = crounchRadius;

                groundCheckPosition.localPosition = newGroundCheckPosition;

                //event
                OnCrounch?.Invoke(crounching);

                if (crounchScript != null)
                {
                    crounchScript.SetCrounching(false);
                }

                SetMovementValues();
            }
            else
            {

                if (crounchScript != null)
                {
                    if (crounchScript.Verify() == false)
                    {
                        BackToStand();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (Verify() == false)
                    {
                        BackToStand();
                    }
                }
            }
        }
    }

    private void BackToStand()
    {
        crounching = false;

        characterController.height = defaltHeight;

        characterController.center = defaltCenter;

        characterController.radius = defaltRadius;

        groundCheckPosition.localPosition = defaltGroundCheckPosition;

        //event
        OnCrounch?.Invoke(crounching);

        SetMovementValues();
    }

    public override void SetCrounching(bool value)
    {
        crounching = value;

        OnCrounch?.Invoke(crounching);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(startPosition.position, startPosition.position + (Vector3.up * raycastLength));
        }
    }
}
