using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrounchHoldButton : CrounchCharacter
{
    private Coroutine waitingVerify;

    protected override void Awake()
    {
        base.Awake();

        inputController.OnCrounching += InputController_OnCrounch;

        inputController.OnCrounchCancel += InputController_OnCrounchCancel;
    }

    private void OnDisable()
    {
        if(waitingVerify!=null)
        StopCoroutine(waitingVerify);

        crounching = false;

        OnCrounch?.Invoke(crounching);

        SetMovementValues();
    }

    private void OnEnable()
    {
        if (waitingVerify != null)
            StopCoroutine(waitingVerify);

        crounching = false;

        characterController.height = defaltHeight;

        characterController.center = defaltCenter;

        characterController.radius = defaltRadius;

        groundCheckPosition.localPosition = defaltGroundCheckPosition;

        //event
        OnCrounch?.Invoke(crounching);

        SetMovementValues();
    }

    protected override void InputController_OnCrounch()
    {
        if (rayCheck.SphereCheck(groundCheckPosition.position, 0, layerMask) == true)
        {
            if (crounching == false)
            {
                if (waitingVerify != null)
                    StopCoroutine(waitingVerify);

                CrounchAction();
            }
        }
    }

    protected void InputController_OnCrounchCancel()
    {
        if (crounching == true)
        {
            if (rayCheck.SphereCheck(groundCheckPosition.position, 0, layerMask) == true)
            {
                if (Verify() == false)
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
                else
                {
                    waitingVerify = StartCoroutine(KeepVerify());
                }
            }
        }
    }

    private void CrounchAction()
    {
        crounching = true;

        characterController.height = crounchHeight;

        characterController.center = crounchCenter;

        characterController.radius = crounchRadius;

        groundCheckPosition.localPosition = newGroundCheckPosition;

        //event
        OnCrounch?.Invoke(crounching);

        SetMovementValues();
    }


    public override void SetCrounching(bool value)
    {
        crounching = value;

        OnCrounch?.Invoke(crounching);
    }

    IEnumerator KeepVerify()
    {
        do
        { 
            yield return null;

        } while (Verify() == true);

        crounching = false;

        characterController.height = defaltHeight;

        characterController.center = defaltCenter;

        characterController.radius = defaltRadius;

        groundCheckPosition.localPosition = defaltGroundCheckPosition;

        //event
        OnCrounch?.Invoke(crounching);

        SetMovementValues();

        yield break;
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

