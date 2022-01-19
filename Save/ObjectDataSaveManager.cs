using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataSaveManager:MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private List<SavebleObject> savebleObjects;

    private void Start()
    {
        //SavebleObject[] temp = FindObjectsOfType<SavebleObject>();

        //foreach (SavebleObject s in temp)
        //{
        //    savebleObjects.Add(s);
        //}

        TryLoadData();
    }

    #region Objects
    public void SaveObjectsData()
    {     
        ObjectsData objectsData = new ObjectsData();

        objectsData.objectName = new string[savebleObjects.Count];
        objectsData.objectEnabled = new bool[savebleObjects.Count];
        objectsData.colliderEnabled = new bool[savebleObjects.Count];
        objectsData.colliderIsTrigger = new bool[savebleObjects.Count];

        objectsData.positionX = new float[savebleObjects.Count];
        objectsData.positionY = new float[savebleObjects.Count];
        objectsData.positionZ = new float[savebleObjects.Count];

        objectsData.rotationX = new float[savebleObjects.Count];
        objectsData.rotationY = new float[savebleObjects.Count];
        objectsData.rotationZ = new float[savebleObjects.Count];

        for (int i = 0; i < savebleObjects.Count; i++)
        {
            objectsData.objectName[i] = savebleObjects[i].GetName();
            objectsData.objectEnabled[i] = savebleObjects[i].IsEnabled();

            objectsData.positionX[i] = savebleObjects[i].GetPosition().x;
            objectsData.positionY[i]= savebleObjects[i].GetPosition().y;
            objectsData.positionZ[i]= savebleObjects[i].GetPosition().z;

            objectsData.rotationX[i] = savebleObjects[i].GetRotation().x;
            objectsData.rotationY[i] = savebleObjects[i].GetRotation().y;
            objectsData.rotationZ[i] = savebleObjects[i].GetRotation().z;


            objectsData.colliderEnabled[i] = savebleObjects[i].ColliderIsActive();
            objectsData.colliderIsTrigger[i] = savebleObjects[i].ColliderIsTrigger();
           
        }

        string path;

        Save.SaveObjectData(objectsData,out path);

        levelData.objectDataloadFilePath = path;
        levelData.hasSaveData = true;

        print("Saved");
    }

    public void TryLoadData()
    {
        if (levelData.hasSaveData)
        {
            ObjectsData objectsData = Save.LoadObjectlData(levelData.objectDataloadFilePath);

            LoadObjectsData(objectsData);
        }
        else
        {
            print("No data");
        }
            
    }

    #endregion

    private void LoadObjectsData(ObjectsData data)
    {
        for(int i = 0; i < data.objectName.Length; i++)
        {
            int index = savebleObjects.FindIndex(0, x => x.gameObject.name == data.objectName[i]);

            if (index >= 0)
            {
                GameObject obj = savebleObjects[index].gameObject;
                Collider objCollider;

                if (obj)
                {
                    print(obj.name + "/" + data.objectEnabled[i]);

                    obj.transform.position = new Vector3(data.positionX[i], data.positionY[i], data.positionZ[i]);

                    obj.transform.localEulerAngles = new Vector3(data.rotationX[i], data.rotationY[i], data.rotationZ[i]);

                    obj.SetActive(data.objectEnabled[i]);

                    if (obj.TryGetComponent<Collider>(out objCollider))
                    {
                        objCollider.enabled = data.colliderEnabled[i];
                        objCollider.isTrigger = data.colliderIsTrigger[i];
                    }
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
        Save.DeleteData(levelData.objectDataloadFilePath);

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
public class ObjectsData
{
    public string[] objectName;
    public bool[] objectEnabled;

    public bool[] colliderEnabled;
    public bool[] colliderIsTrigger;
    
    public float[] positionX;
    public float[] positionY;
    public float[] positionZ;

    public float[] rotationX;
    public float[] rotationY;
    public float[] rotationZ;
}





