using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level data", menuName = "Assets/New level data")]
public class LevelData : ScriptableObject
{
    public bool hasSaveData;
    public string objectDataloadFilePath;
    public string interagibleDataloadFilePath;
    public string inventoryDataPath;
    public string lifesAmountPath;
}
