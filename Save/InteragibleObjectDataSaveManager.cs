using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteragibleObjectDataSaveManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private List<SavebleInteractionData> savebleObjects;


    private void Start()
    {
        //SavebleInteractionData[] temp = FindObjectsOfType<SavebleInteractionData>();

        //foreach (SavebleInteractionData s in temp)
        //{
        //    savebleObjects.Add(s);
        //}

        TryLoadData();
    }

  

    #region Objects
    public void SaveObjectsData()
    {
        InteragibleData data = new InteragibleData();

        data.objectName = new string[savebleObjects.Count];
        data.alreadyInteract = new bool[savebleObjects.Count];


        for (int i = 0; i < savebleObjects.Count; i++)
        {
            data.objectName[i] = savebleObjects[i].GetName();
            data.alreadyInteract[i] = savebleObjects[i].GetInteractiveState();                  
        }

        string path;

        Save.SaveInteragibleData(data, out path);

        levelData.interagibleDataloadFilePath = path;
        levelData.hasSaveData = true;

        print("Saved");
    }

    public void TryLoadData()
    {
        if (levelData.hasSaveData)
        {
            InteragibleData objectsData = Save.LoadInteragiblelData(levelData.interagibleDataloadFilePath);

            LoadObjectsData(objectsData);
        }
        else
        {
            print("No data");
        }

    }

    #endregion

    private void LoadObjectsData(InteragibleData data)
    {
        for (int i = 0; i < data.objectName.Length; i++)
        {

            int index = savebleObjects.FindIndex(0, x => x.gameObject.name == data.objectName[i]);

            if (index >= 0)
            {
                GameObject obj = savebleObjects[index].gameObject;

                if (obj)
                {
                    obj.GetComponent<IinteractiveData>()?.SetData(data.alreadyInteract[i]);
                }
            }
            else
            {
                print("Obj null: " + data.objectName[i]);
            }
        }
    }

    public void Delete()
    {
        Save.DeleteData(levelData.interagibleDataloadFilePath);

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
public class InteragibleData
{
    public string[] objectName;
    public bool[] alreadyInteract;
}


