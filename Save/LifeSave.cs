using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSave : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Respawn respawn;


    private void Start()
    {
        
        TryLoadData();
    }

    #region Objects
    public void SaveData()
    {
        Lifes lifes = new Lifes();

        lifes.amount = respawn.GetCurrentLife();

        string path;

        Save.SaveLife(lifes, out path);

        levelData.lifesAmountPath = path;
        levelData.hasSaveData = true;

        print("Saved");
    }

    public void TryLoadData()
    {
        if (levelData.hasSaveData)
        {
            Lifes data = Save.LoadLifeAmount(levelData.lifesAmountPath);

            respawn.SetCurrentLife(data.amount);
           
        }
        else
        {
            print("No data");
        }

    }

    #endregion

   

    public void Delete()
    {
        Save.DeleteData(levelData.lifesAmountPath);

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
public class Lifes
{
    public int amount;
}