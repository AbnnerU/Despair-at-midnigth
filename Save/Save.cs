using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save
{

    #region DefaltObjects
    public static void SaveObjectData(ObjectsData objectsData, out string pathSaved)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.level";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, objectsData);

        stream.Close();

        pathSaved = path;
    }

    public static ObjectsData LoadObjectlData(string path)
    {

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            ObjectsData objectsData = formatter.Deserialize(stream) as ObjectsData;

            stream.Close();

            return objectsData;
        }
        else
        {
            Debug.LogError(path + " not exist");

            return null;
        }

    }

    public static void SaveInteragibleData(InteragibleData data, out string pathSaved)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/interagbleSave.level";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();

        pathSaved = path;
    }

    public static InteragibleData LoadInteragiblelData(string path)
    {

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            InteragibleData data = formatter.Deserialize(stream) as InteragibleData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError(path + " not exist");

            return null;
        }

    }

    public static void SaveInventoryData(InventoryData data, out string pathSaved)
    {
        //BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/inventory.json";

        string content = JsonUtility.ToJson(data);

        File.WriteAllText(path, content);

        //FileStream stream = new FileStream(path, FileMode.Open);

        //formatter.Serialize(stream, data);

        //stream.Close();

        pathSaved = path;
    }

    public static InventoryData LoadInventoryData(string path)
    {

        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();

            //FileStream stream = new FileStream(path, FileMode.Open);

            //InventoryData data = formatter.Deserialize(stream) as InventoryData;

            //stream.Close();
            string content = File.ReadAllText(path);

            InventoryData data = JsonUtility.FromJson<InventoryData>(content);

            return data;
        }
        else
        {
            Debug.LogError(path + " not exist");

            return null;
        }

    }

    #endregion

    public static void SaveLife(Lifes lifeAmount, out string pathSaved)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/life.level";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, lifeAmount);

        stream.Close();

        pathSaved = path;
    }

    public static Lifes LoadLifeAmount(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            Lifes data = formatter.Deserialize(stream) as Lifes;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError(path + " not exist");

            return null;
        }

    }


    public static void DeleteData(string path)
    {      

        if (File.Exists(path))
        {
            File.Delete(path);
        }


    }

   

   
}


