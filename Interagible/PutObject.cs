using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutObject : MonoBehaviour, IinteractiveTarget,IinteractiveData
{
    [SerializeField] private bool active = true;

    [SerializeField] private InteractionConfig interactionConfig;

    [SerializeField] private InventoryItem itemToInteract;

    [SerializeField] private Transform positionReference;

    [SerializeField] private Vector3 rotation;

    [SerializeField] private bool removeItem=true;

    [SerializeField] private bool enableItem=true;

    [SerializeField] private bool disableItemColider=true;

    [SerializeField] private string inventoryTag = "Inventory";

    [TextArea]
    [SerializeField] private string inputDescription;

    [TextArea]
    [SerializeField] private string noItem;

    public InteractAction OnInteract;

    private Inventory inventoryReference;

    private bool alreadyInteract = false;

    private string popUpText;

    public string PopupText
    {
        get { return popUpText; }
    }

    private void Awake()
    {
        inventoryReference = GameObject.FindGameObjectWithTag(inventoryTag).GetComponent<Inventory>();
    }


    public void Action()
    {
        if (active)
        {
            if (inventoryReference.HaveItem(itemToInteract) == false)
                return;

            if (interactionConfig == InteractionConfig.ONE_INTERACTION && alreadyInteract == false)
            {
                Transform objTransform = inventoryReference.GetItemTransform(itemToInteract);
                Collider collider = objTransform.GetComponent<Collider>();

                if (objTransform == null)
                {
                    Debug.LogError("Transform null");
                    return;
                }

                objTransform.gameObject.SetActive(enableItem);

                objTransform.position = positionReference.position;

                objTransform.rotation = Quaternion.Euler(rotation);

                if (collider)
                    collider.enabled = !disableItemColider;

                if (removeItem)
                    inventoryReference.RemoveItem(itemToInteract);

                OnInteract?.Invoke();
            }
        }
    }

    public void OnLookingAtObject()
    {
        if (active)
        {
            if (inventoryReference.HaveItem(itemToInteract))
            {
                popUpText = inputDescription;
            }
            else
            {
                popUpText = noItem;
            }
        }
      
    }

    public void OnStopLook()
    {
        //
    }

    public void SetActive(bool active)
    {
        this.active = active;
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

