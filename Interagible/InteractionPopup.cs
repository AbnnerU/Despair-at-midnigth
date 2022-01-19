
using UnityEngine;
using TMPro;

public class InteractionPopup : MonoBehaviour
{
    [Header("Canvas")]

    [SerializeField] private TMP_Text popUptext;

    private InteractionPopUp interactEvent;

    private string currentDescription="";

    private void Awake()
    {
        interactEvent = FindObjectOfType<InteractionPopUp>();

        interactEvent.OnNewInteractiveTarget += ObjectInteractiveEvent_OnNewInteractiveTarget;
    }


    private void ObjectInteractiveEvent_OnNewInteractiveTarget(IinteractiveTarget target)
    {
        if (target != null)
        {
            currentDescription = target.PopupText;

            popUptext.text = currentDescription;

        }
        else
        {
            currentDescription = "";

            popUptext.text = currentDescription;
        }

    }
}
