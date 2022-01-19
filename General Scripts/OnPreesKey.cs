using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[System.Serializable] public class OnPressKey : UnityEvent { }


public class OnPreesKey : MonoBehaviour
{
    [SerializeField] private InputActionReference input;
    [SerializeField] private InputMapActive inputType;
    public OnPressKey onPressKey;

    private InputController inputController;


    private void Awake()
    {
        inputController = FindObjectOfType<InputController>();

        input.action.Enable();
        input.action.performed += ctx => CallEvent();
    }

    private void CallEvent()
    {
        if (inputController)
        {
            if (inputType == inputController.GetInputActived())
            {
                //print("Event");
                onPressKey?.Invoke();
            }
        }
        else
        {
            //print("Event");
            onPressKey?.Invoke();
        }
    }
}
