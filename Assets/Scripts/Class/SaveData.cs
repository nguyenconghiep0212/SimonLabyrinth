using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string currentScene;

    // PLAYER PROPERTIES
    public GameManager.PlayableCharacter characterID;
    public Vector3 playerPosition;
    public int playerWeaponID;
    public int medBagCount;
    public int batteryCount;
    public int levelCount;
    public float currentHealth;
    public float currentMana;
    public float currentExp;
    public float maxHealth;
    public float maxMana;
    public float reqExp;

    // STATS
    public int goldCount;
    public int killCount;
}
