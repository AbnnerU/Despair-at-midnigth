using System;
using UnityEngine;


public class InputController : MonoBehaviour
{
    private Controls controls;

    //Gameplay
    public Action<Vector2> OnMoveEvent;

    public Action OnJumpEvent;

    public Action<float> OnLeaningRightEvent;

    public Action<float> OnLeaningLeftEvent;

    public Action OnInstantlyTurnBack;

    public Action<float> OnRun;

    public Action OnCrounching;

    public Action OnCrounchCancel;

    public Action OnProne;

    public Action OnInteract;

    public Action OnOpenObjectives;

    public Action OnControlBreath;

    //Inventory
    public Action OnExit;
    public Action OnNextPage;
    public Action OnBackPage;


    //Pause
    public Action OnPause;

    private InputMapActive inputActived;

    private void Awake()
    {
        controls = new Controls();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        inputActived = InputMapActive.GAMEPLAY;

        //---------------Gameplay---------------
        //Perfomed
        controls.Gameplay.Movement.performed += ctx => Movement_input(ctx.ReadValue<Vector2>());

        //controls.Gameplay.Jump.performed += ctx => Jump_input();

        controls.Gameplay.LeaningRight.performed += ctx => LeaningRigth_Input(ctx.ReadValue<float>());

        controls.Gameplay.LeaningLeft.performed += ctx => LeaningLeft_Input(ctx.ReadValue<float>());

        controls.Gameplay.InstantlyTurnBack.performed += ctx => InstantlyTurnBack_Input();

        controls.Gameplay.Run.performed += ctx => Run_Input(ctx.ReadValue<float>());

        controls.Gameplay.Crounching.performed += ctx => Crounch_Input();

        ////controls.Gameplay.Prone.performed += ctx => Prone_Input();

        controls.Gameplay.Interact.performed += ctx => Interact_Input();

        controls.Gameplay.OpenObjectives.performed += ctx => OpenObjectives_Input();

        controls.Gameplay.BreathControl.performed += ctx => ControlBreath_Input();

        controls.Gameplay.Pause.performed += ctx => Pause_Input();

        //Canceled
        controls.Gameplay.Movement.canceled += ctx => Movement_input(ctx.ReadValue<Vector2>());

        //controls.Gameplay.Jump.canceled += ctx => Jump_input();

        controls.Gameplay.LeaningRight.canceled += ctx => LeaningRigth_Input(ctx.ReadValue<float>());

        controls.Gameplay.LeaningLeft.canceled += ctx => LeaningLeft_Input(ctx.ReadValue<float>());

        controls.Gameplay.Run.canceled += ctx => Run_Input(ctx.ReadValue<float>());

        controls.Gameplay.Crounching.canceled += ctx => CrounchCancel_Input();







        //---------------Inventory---------------
        //Perfomed
        controls.Inventory.Exit.performed += ctx => Exit_Input();

        controls.Inventory.NextPage.performed += ctx => NextPage_Input();

        controls.Inventory.BackPage.performed += ctx => BackPage_Input();





        //---------------Pause---------------
        //Perfomed
        controls.Pause.ExitPause.performed += ctx => Pause_Input();




        //--------------Adicional  Pause----------------
        controls.PauseAdicional.Pause.performed += ctx => Pause_Input();
    }

    private void ControlBreath_Input()
    {
        OnControlBreath?.Invoke();
    }

    public void EnableInventoryInputs()
    {
        controls.Gameplay.Disable();

        controls.Pause.Disable();

        controls.Inventory.Enable();

        inputActived = InputMapActive.INVENTORY;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableGameplayInputs()
    {
        controls.Inventory.Disable();

        controls.Pause.Disable();

        controls.Gameplay.Enable();
      
        inputActived = InputMapActive.GAMEPLAY;

        Cursor.lockState = CursorLockMode.Locked;

    }

    public void EnablePauseInputs()
    {
        controls.Inventory.Disable();

        controls.Gameplay.Disable();

        controls.Pause.Enable();

        inputActived = InputMapActive.PAUSE;

        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableControlsDisabledInputs()
    {
        controls.Inventory.Disable();

        controls.Gameplay.Disable();

        controls.Pause.Disable();

        controls.PauseAdicional.Enable();

        inputActived = InputMapActive.CONTROLSDISABLED;

        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Gameplay
    private void Movement_input(Vector2 ctx)
    {
        //print(ctx);

        OnMoveEvent?.Invoke(ctx);
    }

    private void Jump_input()
    {
        
         OnJumpEvent?.Invoke();
        
    }

    private void LeaningRigth_Input(float ctx)
    {
        OnLeaningRightEvent?.Invoke(ctx);
    }

    private void LeaningLeft_Input(float ctx)
    {
        OnLeaningLeftEvent?.Invoke(ctx);
    }

    private void InstantlyTurnBack_Input()
    {
        OnInstantlyTurnBack?.Invoke();
    }

    private void Run_Input(float ctx)
    {
        OnRun?.Invoke(ctx);
    }

    private void Crounch_Input()
    {
        OnCrounching?.Invoke();
    }

    private void CrounchCancel_Input()
    {
        OnCrounchCancel?.Invoke();
    }

    private void Prone_Input()
    {
        OnProne?.Invoke();
    }

    private void Interact_Input()
    {
        OnInteract?.Invoke();
    }

    private void OpenObjectives_Input()
    {
        OnOpenObjectives?.Invoke();
    }
    #endregion



    #region Inventory
    private void BackPage_Input()
    {
        OnBackPage?.Invoke();
    }

    private void NextPage_Input()
    {
        OnNextPage?.Invoke();
    }

    private void Exit_Input()
    {
        OnExit?.Invoke();
    }

    #endregion


    private void Pause_Input()
    {
        OnPause?.Invoke();
    }

    public InputMapActive GetInputActived()
    {
        return inputActived;
    }
}


public enum InputMapActive
{
    GAMEPLAY,
    INVENTORY,
    PAUSE,
    CONTROLSDISABLED
}