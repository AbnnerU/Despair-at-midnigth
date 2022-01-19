using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour, IinteractiveTarget, IinteractiveData
{
    [SerializeField] private InventoryItem keyReference;

    [SerializeField] private int numberOfLocks=1;

    [SerializeField] private Rigidbody doorRigidbody;

    [TextArea]
    [SerializeField] private string inputDescription;

    [TextArea]
    [SerializeField] private string noKey;

    public InteractAction OnInteract;

    public InteractAction OnLockedDoor;

    private Inventory inventoryReference;

    private Transform _transform;

    private int currentLockNumbers;

    private bool doorOpen = false;

    private string popUpText;

    public string PopupText
    {
        get { return popUpText; }
    }

    private void Awake()
    {
        if (numberOfLocks <= 0)
            numberOfLocks = 1;

        if (doorRigidbody == null)
            doorRigidbody = GetComponent<Rigidbody>();

        _transform = GetComponent<Transform>();
        inventoryReference = GameObject.FindGameObjectWithTag(keyReference.inventoryTag).GetComponent<Inventory>();

        _transform.localRotation = Quaternion.Euler(0,0,0);

        doorRigidbody.freezeRotation = true;

        currentLockNumbers = numberOfLocks;
    }

    public void Action()
    {
        if (inventoryReference != null && inventoryReference.HaveItem(keyReference))
        {
            inventoryReference.RemoveItem(keyReference);

            currentLockNumbers--;

            if (currentLockNumbers == 0)
            {
                OpenDoor();

                OnInteract?.Invoke();
            }
        }
    }

    public void OnLookingAtObject()
    {
        if (doorOpen == false)
        {
            if (inventoryReference != null && inventoryReference.HaveItem(keyReference))
            {
                popUpText = inputDescription;
            }
            else
            {
                OnLockedDoor?.Invoke();

                popUpText = noKey;
            }
        }
        else
        {
            popUpText = "";
        }
    }

    public void OnStopLook()
    {
        //
    }

    public void OpenDoor()
    {
        popUpText = "";

        doorOpen = true;

        doorRigidbody.freezeRotation = false;
    }

    public bool AlreadyInteracted()
    {
        return doorOpen;
    }

    public void SetData(bool value)
    {
        doorOpen = value;

        if (doorOpen)
        {
            OpenDoor();
        }
    }
}
