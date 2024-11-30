using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    [SerializeField] internal List<SaveOption> allSaveSlots;
    [SerializeField] internal SaveOption currentSave;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (SaveOption slot in allSaveSlots)
        //{
        //    slot.FillSavedContentToUI();
        //}
    }


    public void Save()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            goldCount = GameManager.Instance.gold,
            killCount = GameManager.Instance.killCount,
            levelCount = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().level,
            characterID = "heinrich_von_kropp",
            //playerWeaponID = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentWeapon,
            medBagCount = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().medbag,
            batteryCount = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().battery,
            currentScene = SceneManager.GetActiveScene().name,
        };
        File.WriteAllText(currentSave.savePath, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (currentSave.isUsed)
        {
            LoadOldSave();
        }
        else
        {
            CreateNewSave();
        }

        MenuManager.Instance.mainMenuUI.SetActive(false);
        GameManager.Instance.PlayerUI.SetActive(true);
        GameManager.Instance.StatUI.SetActive(true);
    }

    private void LoadOldSave()
    {
        foreach (SaveOption slot in allSaveSlots)
        {
            slot.inCurrentUse = false;
        }


        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(currentSave.savePath));
        StartCoroutine(LoadSceneAndApplyData(saveData));
    }
    private IEnumerator LoadSceneAndApplyData(SaveData saveData)
    {
        // Load the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(saveData.currentScene);

        // Wait until the scene is fully loaded
        while (!asyncOperation.isDone)
        {
            yield return null; // Wait for the next frame
        }

        // Now the scene is loaded, find the player
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            // Set player properties
            player.transform.position = saveData.playerPosition;
            GameManager.Instance.UpdateGold(saveData.goldCount);
            GameManager.Instance.UpdateKillCount(saveData.killCount);
            player.level = saveData.levelCount;
            GameManager.Instance.levelUI.text = player.level.ToString("N0");
            //character = "heinrich_von_kropp",
            //player.playerControl.ChangeWeapon(saveData.playerWeapon);
            player.medbag = saveData.medBagCount;
            GameManager.Instance.medbagUI.text = player.medbag.ToString();
            player.battery = saveData.batteryCount;
            GameManager.Instance.batteryUI.text = player.battery.ToString();
        }
        else
        {
            Debug.LogError("Player not found after loading scene.");
        }

        currentSave.inCurrentUse = true;
    }




    private void CreateNewSave()
    {
        currentSave.isUsed = true;
        currentSave.inCurrentUse = true;
        Debug.Log("New Save created !!");

        SaveData saveData = new SaveData
        {
            currentScene = "Game",
        };
        File.WriteAllText(currentSave.savePath, JsonUtility.ToJson(saveData));
    }
}
