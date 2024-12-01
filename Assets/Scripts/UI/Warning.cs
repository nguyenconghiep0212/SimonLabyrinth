using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warning : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void QuitToMenuScreen()
    {
        MenuManager.Instance.playButtonUI.SetActive(true);
        MenuManager.Instance.resumeButtonUI.SetActive(false);
        MenuManager.Instance.closeButtonUI.SetActive(false);
        MenuManager.Instance.mainMenuUI.SetActive(true);


        SaveManager.Instance.currentSave = null;

        GameManager.Instance.PlayerUI.SetActive(false);
        GameManager.Instance.StatUI.SetActive(false);
        GameManager.Instance.VendorUI.SetActive(false);

        foreach (SaveOption slot in SaveManager.Instance.allSaveSlots)
        {
            slot.FillSavedContentToUI();
        }

        Close();
        SceneManager.LoadScene("Menu");

    }
}
