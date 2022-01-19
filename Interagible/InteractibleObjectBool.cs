using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObjectBool : MonoBehaviour, IinteractiveTarget, IinteractiveData
{
    [SerializeField] private InteractionConfigBool interactionConfig;

    [SerializeField] private bool boolValue;

    [SerializeField] private string inputDescription;

    public InteractAction<bool> OnInteract;

    public string InteractionDescription { get { return inputDescription; } }

    private bool alreadyInteract=false;

    public string PopupText
    {
        get { return inputDescription; }
    }

    public void Action()
    {       
        if(interactionConfig == InteractionConfigBool.ONE_INTERACTION && alreadyInteract == false)
        {
            OnInteract?.Invoke(boolValue);
        }
        else if(interactionConfig == InteractionConfigBool.UNLIMITED)
        {
            OnInteract?.Invoke(boolValue);
        }
        else if(interactionConfig == InteractionConfigBool.UNLIMITED_INVERTBOOL_VALUE)
        {
            OnInteract?.Invoke(boolValue);
            boolValue = !boolValue;
        }

       if(alreadyInteract==false)
            alreadyInteract = true;
       
    }

    public void OnLookingAtObject()
    {
        print("Enter");
        
    }

    public void OnStopLook()
    {
        print("Exit");
        
    }

    public bool AlreadyInteracted()
    {
        return alreadyInteract;
    }

    public void SetData(bool value)
    {
        alreadyInteract = value;
    }
}
   
