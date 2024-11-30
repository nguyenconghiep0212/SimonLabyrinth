using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public Vector3 playerPosition;
    public int goldCount;
    public int killCount;
    public int levelCount;
    public int medBagCount;
    public int batteryCount;
    public string characterID;
    public string playerWeaponID;
    public string currentScene;
}
