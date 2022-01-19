using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDataSaveManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private List<Inventory> inventorys;

    private void Start()
    {
        Inventory[] temp = GameObject.FindObjectsOfType<Inventory>();

        foreach(Inventory i in temp)
        {
            inventorys.Add(i);
        }

        TryLoadData();
    }

    #region Objects
    public void SaveObjectsData()
    {
        InventoryData data = new InventoryData();

        data.inventoryName = new string[inventorys.Count];
        data.itemData = new ItemData[inventorys.Count];

        for(int i = 0; i < inventorys.Count; i++)
        {
            data.inventoryName[i] = inventorys[i].gameObject.name;

            ItemData currentItemData = new ItemData();

            currentItemData.item = new InventoryItem[inventorys[i].GetAllKeys().Length];
            currentItemData.itemTransform = new Transform[inventorys[i].GetAllValues().Length];

            int id = 0;

            foreach(InventoryItem item in inventorys[i].GetAllKeys())
            {             
                if (item!=null)
                    currentItemData.item[id] = item;

                id++;
            }

            id = 0;

            foreach(Transform t in inventorys[i].GetAllValues())
            {                
                if (t!=null)
                   currentItemData.itemTransform[id] = t;

                id++;
            }


            data.itemData[i] = currentItemData;
        }

        string path;

        Save.SaveInventoryData(data,out path);

        levelData.inventoryDataPath = path;

        print("Saved");
    }

    public void TryLoadData()
    {
        if (levelData.hasSaveData)
        {
            InventoryData data = Save.LoadInventoryData(levelData.inventoryDataPath);

            LoadInventoryData(data);
        }
        else
        {
            print("No data");
        }

    }

    #endregion

    private void LoadInventoryData(InventoryData data)
    {
       for(int i = 0; i < data.inventoryName.Length; i++)
       {
            int index = inventorys.FindIndex(0, x => x.gameObject.name == data.inventoryName[i]);

            if (index >= 0)
            {
                for(int y = 0; y < data.itemData[i].item.Length; y++)
                {
                    inventorys[index].AddItem(data.itemData[i].item[y], data.itemData[i].itemTransform[y]);
                }
            }
            else
            {
                print("Inventory null: " + data.inventoryName);
            }
        }
    }

    public void Delete()
    {
        Save.DeleteData(levelData.inventoryDataPath);

        levelData.hasSaveData = false;
    }

    public void DeleteAll()
    {
        Save.DeleteData(levelData.interagibleDataloadFilePath);
        Save.DeleteData(levelData.objectDataloadFilePath);
        Save.DeleteData(levelData.inventoryDataPath);
        Save.DeleteData(levelData.lifesAmountPath);

        levelData.hasSaveData = false;
    }
}

[System.Serializable]
public class InventoryData
{
    public string[] inventoryName;
    public ItemData[] itemData;
}

[System.Serializable]
public class ItemData
{
    public InventoryItem[] item;
    public Transform[] itemTransform;
}