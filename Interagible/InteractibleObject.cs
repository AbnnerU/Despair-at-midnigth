using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour, IinteractiveTarget, IinteractiveData
{
    [SerializeField] private InteractionConfig interactionConfig;

    [SerializeField] private string inputDescription;

    public InteractAction OnInteract; 

    private bool alreadyInteract=false;

    private string popupText;

    public string PopupText
    {
        get { return popupText; }
    }

    private void Awake()
    {
        popupText = inputDescription;
    }

    public void Action()
    {
        if (interactionConfig == InteractionConfig.ONE_INTERACTION && alreadyInteract == false)
        {
            OnInteract?.Invoke();

            popupText = "";
        }
        else if (interactionConfig == InteractionConfig.UNLIMITED)
        {
            OnInteract?.Invoke();
        }

       if(alreadyInteract==false)
            alreadyInteract = true;
       
    }

    public void OnLookingAtObject()
    {
        //print("Enter");
        
    }

    public void OnStopLook()
    {
        //print("Exit");
        
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
   
