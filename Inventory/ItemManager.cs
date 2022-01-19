using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour, IinteractiveTarget,IinteractiveData
{
    [SerializeField] private bool active = true;

    [SerializeField] private InventoryItem itemAttributes;

    [TextArea]
    [SerializeField] private string inputDescription;

    [TextArea]
    [SerializeField] private string noInventorySpace;

    public InteractAction OnInteract;

    private Inventory inventoryReference;

    private string popUpText;

    public string PopupText
    {
        get { return popUpText; }
    }

    private void Awake()
    {
        inventoryReference = GameObject.FindGameObjectWithTag(itemAttributes.inventoryTag).GetComponent<Inventory>();

        if (inventoryReference == null)
            print(GameObject.FindGameObjectWithTag(itemAttributes.inventoryTag).name);
    }

    public void Action()
    {
        if (active == false)
            return;


        if (inventoryReference.CanAddNewItem() == false)
            return;

        inventoryReference.AddItem(itemAttributes,transform);

        OnInteract?.Invoke();
    }

    public void OnLookingAtObject()
    {
        if (active == false)
        { 
            popUpText = "";
            return;
        }

        if (inventoryReference.CanAddNewItem())
        {
            popUpText = inputDescription;
        }
        else
        {
            popUpText = noInventorySpace;
        }
    }

    public void OnStopLook()
    {
        //
    }


    public void SetActive(bool value)
    {
        active = value;
    }

    public bool AlreadyInteracted()
    {
        return active;
    }

    public void SetData(bool value)
    {
        active = value;
    }
}
