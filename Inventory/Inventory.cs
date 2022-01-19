using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int numberOfSlots;

    private Dictionary<InventoryItem,Transform> items = new Dictionary<InventoryItem, Transform>();

    public Action<InventoryItem> OnAddNewItem;

    public Action<InventoryItem> OnRemoveItem;

    public bool CanAddNewItem()
    {
        if(items.Count < numberOfSlots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddItem(InventoryItem newItem, Transform itemTransform)
    {
        if (CanAddNewItem() == false)
            return;

        items.Add(newItem,itemTransform);

        OnAddNewItem?.Invoke(newItem);       
    }

    public void RemoveItem(InventoryItem itemReference)
    {
        if (HaveItem(itemReference) == false)
            return;

        items.Remove(itemReference);


        OnRemoveItem?.Invoke(itemReference);
    }

    public bool HaveItem(InventoryItem itemReference)
    {
        if (items.ContainsKey(itemReference))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Transform GetItemTransform(InventoryItem key)
    {
        if (HaveItem(key) == false)
            return null;

        Transform tempTransform=null;

        items.TryGetValue(key,out tempTransform);

        return tempTransform;
    }

    public Transform[] GetAllValues()
    {
        Transform[] temp = new Transform[items.Values.Count];

        int id = 0;

        foreach(Transform t in items.Values)
        {
            temp[id] = t;
            id++;
        }

        return temp;
    }

    public InventoryItem[] GetAllKeys()
    {
        InventoryItem[] temp = new InventoryItem[items.Keys.Count];

        int id = 0;

        foreach (InventoryItem i in items.Keys)
        {
            temp[id] = i;
            id++;
        }

        return temp;
    }


}
