using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTriggerSound : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        inventory.OnAddNewItem += Inventory_OnAddItem;
      
    }

    private void Inventory_OnAddItem(InventoryItem item)
    {
        audioSource.clip = item.onTakedSound;

        audioSource.Play();

    }
}
