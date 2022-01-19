using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVisual : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private Image[] slotsOrder;

    [SerializeField] private bool disableEmptySlots;

    [SerializeField] private bool disableAllOnStart;

    private Dictionary<InventoryItem, Image> items = new Dictionary<InventoryItem, Image>();

    private void Awake()
    {
        inventory.OnAddNewItem += Inventory_OnAddNewItem;
        inventory.OnRemoveItem += Inventory_OnRemoveItem;

        if (disableAllOnStart)
        {
            for (int i = 0; i < slotsOrder.Length; i++)
            {
                slotsOrder[i].enabled = false;
            }
        }
    }

    private void Inventory_OnAddNewItem(InventoryItem newItem)
    {
        for(int i=0; i < slotsOrder.Length; i++)
        {
            Image image = slotsOrder[i];

            if (image.sprite == null)
            {
                image.sprite = newItem.itemSprite;

                image.enabled = true;

                items.Add(newItem,image);
                break;
            }
        }
    }

    private void Inventory_OnRemoveItem(InventoryItem newItem)
    {
        if (items.ContainsKey(newItem))
        {
            Image itemImage;

            items.TryGetValue(newItem, out itemImage);

            itemImage.sprite = null;

            if (disableEmptySlots)
                itemImage.enabled = false;

            items.Remove(newItem);

        }
        else
        {
            Debug.Log("Not contain: " + newItem.name);
        }
    }


    //private void Reposition(int startId)
    //{      
    //    if (startId < -1)
    //        return;

    //    int count = (items.Count - 1) - startId;

    //    if (count <= 0)
    //        return;

    //    foreach(Image i in items.Values)
    //    {
    //        i
    //    }

    //}


    //private int GetKeyId(InventoryItem reference)
    //{
    //    int id = 0;
    //    foreach(InventoryItem i in items.Keys)
    //    {
    //        if (i == reference)
    //        {
    //            return id;
    //        }
    //        id++;
    //    }

    //    return -1;
    //}
}


