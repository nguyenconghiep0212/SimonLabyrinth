using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
            //DontDestroyOnLoad(Instance);
        }
    }
    [SerializeField] internal GameObject saveGameUI; 
    private string saveLocation;
    // Start is called before the first frame update
    void Start()
    {
        saveGameUI.SetActive(false); 
        saveLocation = Path.Combine(Application.persistentDataPath, "SimonsLabyrinth_SaveData.json");
        LoadGame();
    }

    public void OpenSaveMenu()
    {
        saveGameUI.SetActive(true);
    }

    public void Save()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;
        }
        else
        {
            Save();
        }
    }
}
