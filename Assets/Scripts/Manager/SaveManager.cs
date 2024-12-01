using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class SaveManager : MonoBehaviour
{

    // Static instance of the GameManager
    private static SaveManager instance;
    // Property to access the instance
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<SaveManager>();
                    singletonObject.name = typeof(SaveManager).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    [SerializeField] internal List<SaveOption> allSaveSlots;
    [SerializeField] internal SaveOption currentSave;

    // Start is called before the first frame update
    void Start()
    {
    }


    public void Save()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SaveData saveData = new SaveData
        {
            playerPosition = player.transform.position,
            levelCount = player.level,
            playerWeaponID = player.currentWeapon ? (int)player.currentWeapon.id : -1,
            medBagCount = player.medbag,
            batteryCount = player.battery,
            currentHealth = player.currentHealth,
            currentMana = player.currentMana,
            currentExp = player.currentExp,
            maxHealth = player.maxHealth,
            maxMana = player.maxMana,
            reqExp = player.requiredExp,

            goldCount = GameManager.Instance.gold,
            killCount = GameManager.Instance.killCount,
            characterID = GameManager.Instance.selectedCharacter,
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
        MenuManager.Instance.playButtonUI.SetActive(false);
        MenuManager.Instance.resumeButtonUI.SetActive(true);

        GameManager.Instance.PlayerUI.SetActive(true);
        GameManager.Instance.StatUI.SetActive(true);

    }

    #region ---- || LOAD OLD SAVE || ----
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
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(saveData.currentScene);

        while (!asyncOperation.isDone)
        {
            yield return null; // Wait for the next frame
        }

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            player.transform.position = saveData.playerPosition;
            player.level = saveData.levelCount;
            player.medbag = saveData.medBagCount;
            player.battery = saveData.batteryCount;

            player.currentHealth = saveData.currentHealth;
            player.maxHealth = saveData.maxHealth;
            player.UpdateHealthUI();
            player.currentMana = saveData.currentMana;
            player.maxMana = saveData.maxMana;
            player.UpdateManaUI();
            player.currentExp = saveData.currentExp;
            player.requiredExp = saveData.reqExp;
            player.TakeExperince(player.currentExp);

            if (saveData.playerWeaponID != -1) player.playerControl.ChangeWeapon(GameManager.Instance.GetPlayerSavedWeapon(saveData.playerWeaponID));

            GameManager.Instance.gold = 0;
            GameManager.Instance.UpdateGold(saveData.goldCount);

            GameManager.Instance.killCount = 0;
            GameManager.Instance.UpdateKillCount(saveData.killCount);

            GameManager.Instance.levelUI.text = player.level.ToString("N0");
            GameManager.Instance.medbagUI.text = player.medbag.ToString();
            GameManager.Instance.batteryUI.text = player.battery.ToString();
            GameManager.Instance.selectedCharacter = saveData.characterID;

            GameManager.Instance.InitPlayer();
        }
        else
        {
            Debug.LogError("Player not found after loading scene.");
        }

        currentSave.inCurrentUse = true;
    }
    #endregion

    #region ---- || CREATE NEW SAVE || ----
    private void CreateNewSave()
    {
        currentSave.isUsed = true;
        currentSave.inCurrentUse = true;

        SaveData saveData = new SaveData
        {
            currentScene = "Game",
            characterID = GameManager.Instance.selectedCharacter,
        };
        File.WriteAllText(currentSave.savePath, JsonUtility.ToJson(saveData));
        StartCoroutine(LoadSceneAndInitPlayer(saveData));
    }

    private IEnumerator LoadSceneAndInitPlayer(SaveData saveData)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(saveData.currentScene);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        InitNewGame();
        currentSave.inCurrentUse = true;
    }

    private void InitNewGame()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            GameManager.Instance.InitPlayer();
            player.transform.position = Vector2.zero;
            GameManager.Instance.gold = 1000;
            GameManager.Instance.UpdateGold(0);
            GameManager.Instance.killCount = 0;
            GameManager.Instance.UpdateKillCount(0);
            player.level = 0;
            GameManager.Instance.levelUI.text = player.level.ToString("N0");
            player.medbag = 0;
            GameManager.Instance.medbagUI.text = player.medbag.ToString();
            player.battery = 0;
            GameManager.Instance.batteryUI.text = player.battery.ToString();
        }

    }
    #endregion
}
