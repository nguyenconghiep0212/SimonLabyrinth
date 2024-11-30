using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveOption : MonoBehaviour
{
    [Range(1, 3)]
    [SerializeField] private int id;

    [Header("UI")]
    [SerializeField] private GameObject newSave;
    [SerializeField] private GameObject usedSave;
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private TextMeshProUGUI levelCount;
    [SerializeField] private TextMeshProUGUI medBagCount;
    [SerializeField] private TextMeshProUGUI batteryCount;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Image weaponIcon;

    [SerializeField] internal bool isUsed;
    [SerializeField] internal bool inCurrentUse;
    [SerializeField] internal string savePath;


    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "SimonsLabyrinth_SaveData_" + id + ".json");
        FillSavedContentToUI();
    }
    public void SelectSave()
    {
 
        SaveManager.Instance.currentSave = this;
        SaveManager.Instance.LoadGame();
    }

    public void DeleteSave()
    {
        isUsed = false;
        File.Delete(savePath);
    }

    public void FillSavedContentToUI()
    { 
        if (File.Exists(savePath))
        {
            isUsed = true;
            newSave.SetActive(false);
            usedSave.SetActive(true);
            
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
            goldCount.text = saveData.goldCount.ToString("N0");
            killCount.text = saveData.killCount.ToString("N0");
            levelCount.text = saveData.levelCount.ToString("N0");
            //characterIcon.sprite = 
            //weaponIcon.sprite = saveData.playerWeapon.icon;
            medBagCount.text = saveData.medBagCount.ToString("N0");
            batteryCount.text = saveData.batteryCount.ToString("N0");

        }
        else
        {
            newSave.SetActive(true);
            usedSave.SetActive(false);
        }
    }
}
