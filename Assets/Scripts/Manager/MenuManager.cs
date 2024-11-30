using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; set; }

    [SerializeField] internal GameObject mainMenuUI;
    [SerializeField] internal GameObject menuUI;
    [SerializeField] internal GameObject saveUI;
    [SerializeField] internal GameObject settingUI;

    [SerializeField] internal GameObject closeButtonUI;
    [SerializeField] internal GameObject backButtonUI;

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
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            mainMenuUI.SetActive(false);
        }
        backButtonUI.SetActive(false);
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

    public void BackToMainMenu()
    {
        menuUI.SetActive(true);
        saveUI.SetActive(false);
        settingUI.SetActive(false);
        backButtonUI.SetActive(false);
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
