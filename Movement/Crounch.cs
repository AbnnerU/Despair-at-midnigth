
using UnityEngine;

public class Crounch : CrounchCharacter
{
    [Header("If as prone")]

    //[SerializeField] private bool useAsProne = false;

    [SerializeField] private Prone proneScript;
   
    protected override void Awake()
    {
        base.Awake();
          
        inputController.OnCrounching += InputController_OnCrounch;
        
    }
    
    protected override void InputController_OnCrounch()
    {     
        if (rayCheck.SphereCheck(groundCheckPosition.position, 0, layerMask) == true)
        {
            if (crounching == false)
            {
                if (proneScript != null && proneScript.GetCrounching() == true)
                {
                    //Use proneScript verify to crounch
                    if (proneScript.Verify() == false)
                    {
                        CrounchAction();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    CrounchAction();
                }
                
            }
            else
            {
                if (Verify()==false)
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

        if (proneScript != null)
        {
            proneScript.SetCrounching(false);          
        }


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
