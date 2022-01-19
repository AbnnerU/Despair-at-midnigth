using System;
using UnityEngine.Events;
using UnityEngine;

[Serializable]public class CustomCrounchEvent : UnityEvent<bool> {  };

public abstract class CrounchCharacter : MonoBehaviour
{
    [SerializeField] protected CharacterController characterController;

    [SerializeField] protected RayCheck rayCheck;

    [SerializeField] protected Transform groundCheckPosition;

    [SerializeField] protected float crounchSpeed;

    [SerializeField] protected bool canJump = false;

    [SerializeField] protected bool canRun = false;

    [Header("Defalt Values")]
    [SerializeField] protected float defaltHeight;
    [SerializeField] protected Vector3 defaltCenter;
    [SerializeField] protected Vector3 defaltGroundCheckPosition;
    [SerializeField] protected float defaltRadius;

    [Header("Crounch Values")]
    [SerializeField] protected float crounchHeight;
    [SerializeField] protected Vector3 crounchCenter;
    [SerializeField] protected Vector3 newGroundCheckPosition;
    [SerializeField] protected float crounchRadius;

    [Header("Verification Raycast")]
    [SerializeField] protected bool drawGizmos;
    [SerializeField] protected Transform startPosition;
    [SerializeField] protected float raycastLength;
    [SerializeField] protected LayerMask layerMask;

    public CustomCrounchEvent crounchEvent;

    public Action<bool> OnCrounch;

    protected InputController inputController;

    protected IMovement movement;

    //protected FPSMovement fpsMovement;

    protected bool crounching = false;

    protected RaycastHit[] hitResult;

    protected virtual void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        movement = GetComponent<IMovement>();

        //fpsMovement = GetComponent<FPSMovement>();

        hitResult = new RaycastHit[1];
    }

    protected abstract void InputController_OnCrounch();

    protected virtual void SetMovementValues()
    {
        if (crounching)
        {
            //Event
            crounchEvent?.Invoke(crounching);

            movement.CanRun(canRun);
            movement.CanJump(canJump);
            movement.SetNewSpeed(crounchSpeed);
        }
        else
        {
            //Event
            crounchEvent?.Invoke(crounching);

            movement.CanJump(true);
            movement.CanRun(true);
            movement.BackSpeedToNormal();
        }
    }

    public virtual bool Verify()
    {

        return rayCheck.RayCastCheck(startPosition.position, Vector3.up, raycastLength, layerMask);
    }

    public abstract void SetCrounching(bool value);

 
    public virtual bool GetCrounching()
    {
        return crounching;
    }



}
