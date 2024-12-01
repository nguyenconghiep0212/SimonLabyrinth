using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class MenuManager : MonoBehaviour
{



    // Static instance of the GameManager
    private static MenuManager instance;

    // Property to access the instance
    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<MenuManager>();
                    singletonObject.name = typeof(MenuManager).ToString() + " (Singleton)";
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

    [SerializeField] internal GameObject mainMenuUI;
    [SerializeField] internal GameObject menuUI;
    [SerializeField] internal GameObject saveUI;
    [SerializeField] internal GameObject settingUI;
    [SerializeField] internal GameObject characterUI;

    [SerializeField] internal GameObject closeButtonUI;
    [SerializeField] internal GameObject playButtonUI;
    [SerializeField] internal GameObject resumeButtonUI;
    [SerializeField] internal GameObject backButtonUI;

    [SerializeField] internal GameObject warningUI;
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            mainMenuUI.SetActive(false);
            playButtonUI.SetActive(false);
            resumeButtonUI.SetActive(true);
        }
        else
        {
            closeButtonUI.SetActive(false);
            playButtonUI.SetActive(true);
            resumeButtonUI.SetActive(false);
        }
        backButtonUI.SetActive(false);
        warningUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSaveUI()
    {
        menuUI.SetActive(false);
        saveUI.SetActive(true);
        backButtonUI.SetActive(true);
    }

    public void OpenSettingUI()
    {
        menuUI.SetActive(false);
        settingUI.SetActive(true);
        backButtonUI.SetActive(true);
    }
    public void OpenCharacterSelecetionUI()
    {
        menuUI.SetActive(false);
        saveUI.SetActive(false);
        characterUI.SetActive(true);
        backButtonUI.SetActive(true);
    }
    public void BackToMainMenu()
    {
        menuUI.SetActive(true);
        saveUI.SetActive(false);
        settingUI.SetActive(false);
        characterUI.SetActive(false);
        backButtonUI.SetActive(false);
    }

    public void QuitToMenuScreen()
    {
        mainMenuUI.SetActive(false);
        warningUI.SetActive(true);
    }

    public void CloseMainMenu()
    {
        mainMenuUI.SetActive(false);
        BackToMainMenu();
    }
    public void OpenMainMenu()
    {
        mainMenuUI.SetActive(true);
        BackToMainMenu();
    }
}
