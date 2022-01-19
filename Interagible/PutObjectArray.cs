using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutObjectArray : MonoBehaviour, IinteractiveTarget
{
    [SerializeField] private string inventoryTag = "Inventory";

    [SerializeField] private List<ItemPlacerConfig> itens;

    [SerializeField] private bool canPlaceInduvidualObjcts=true;

    [TextArea]
    [SerializeField] private string inputDescription;

    [TextArea]
    [SerializeField] private string noItem;

    [TextArea]
    [SerializeField] private string dontHaveAllItens;

    public InteractAction OnPutAllItens;

    public InteractAction OnInteract;

    private Inventory inventoryReference;

    private int amount = 0;

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
        if (amount == itens.Count && alreadyInteract==false)
        {
            alreadyInteract = true;
            OnPutAllItens?.Invoke();
            return;
        }
        else if(amount == itens.Count)
        {
            return;
        }

        if (canPlaceInduvidualObjcts)
        {
            for (int i = 0; i < itens.Count; i++)
            {
                if (inventoryReference.HaveItem(itens[i].itemToInteract))
                {
                    Transform objTransform = inventoryReference.GetItemTransform(itens[i].itemToInteract);
                    Collider collider = objTransform.GetComponent<Collider>();

                    if (objTransform == null)
                    {
                        Debug.LogError("Transform null");
                        return;
                    }

                    amount++;

                    objTransform.gameObject.SetActive(itens[i].enableItem);

                    objTransform.position = itens[i].positionReference.position;

                    objTransform.rotation = Quaternion.Euler(itens[i].rotation);

                    if (collider)
                        collider.enabled = !itens[i].disableItemColider;

                    if (itens[i].removeItem)
                        inventoryReference.RemoveItem(itens[i].itemToInteract);

                    OnInteract?.Invoke();
                }
            }
        }
        else
        {
            float itensAmount = 0;

            for (int i = 0; i < itens.Count; i++)
            {
                if (inventoryReference.HaveItem(itens[i].itemToInteract))
                {
                    itensAmount++;                
                }
            }

            if (itensAmount == itens.Count)
            {
                for (int i = 0; i < itens.Count; i++)
                {
                    if (inventoryReference.HaveItem(itens[i].itemToInteract))
                    {
                        Transform objTransform = inventoryReference.GetItemTransform(itens[i].itemToInteract);
                        Collider collider = objTransform.GetComponent<Collider>();

                        if (objTransform == null)
                        {
                            Debug.LogError("Transform null");
                            return;
                        }

                        amount++;

                        objTransform.gameObject.SetActive(itens[i].enableItem);

                        objTransform.position = itens[i].positionReference.position;

                        objTransform.rotation = Quaternion.Euler(itens[i].rotation);

                        if (collider)
                            collider.enabled = !itens[i].disableItemColider;

                        if (itens[i].removeItem)
                            inventoryReference.RemoveItem(itens[i].itemToInteract);

                        OnInteract?.Invoke();
                    }
                }
            }
        }

        if (amount == itens.Count)
        {
            alreadyInteract = true;
            OnPutAllItens?.Invoke();
            print("Acabou");
        }

    }

    public void OnLookingAtObject()
    {
        if (canPlaceInduvidualObjcts)
        {
            for (int i = 0; i < itens.Count; i++)
            {
                if (inventoryReference.HaveItem(itens[i].itemToInteract))
                {
                    popUpText = inputDescription;
                    return;
                }
            }

            popUpText = noItem;
        }
        else
        {
            float itensAmount = 0;

            for (int i = 0; i < itens.Count; i++)
            {
                if (inventoryReference.HaveItem(itens[i].itemToInteract))
                {
                    itensAmount++;
                }
            }

            if (itensAmount != itens.Count)
            {
                popUpText = dontHaveAllItens;
            }
        }

        
    }

    public void OnStopLook()
    {
        //
    }

    //public bool AlreadyInteracted()
    //{
    //    return alreadyInteract;
    //}
}

[System.Serializable]
public struct ItemPlacerConfig
{
    public InventoryItem itemToInteract;

    public Transform positionReference;

    public Vector3 rotation;

    public bool removeItem;

    public bool enableItem;

    public bool disableItemColider;
}