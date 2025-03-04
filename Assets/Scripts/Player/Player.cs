using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [Header("properties")]
    public float maxHealth = 100;
    public float maxMana = 100;
    public float currentHealth;
    public float currentMana;
    public float requiredExp = 100;
    public float currentExp;
    public int level = 0;

    public int medbag = 0;
    public int battery = 0;
    internal bool isDead;
    internal bool inCombat;

    [Header("Weapon")]
    [SerializeField] internal Weapon currentWeapon;
    [SerializeField] internal Weapon hoverWeapon;

    [Header("UI")]
    [SerializeField] internal Transform dialogPosition;

    internal PlayerControl playerControl;
    internal GameObject dialogUI;
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();

    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentExp = 0;
        GameManager.Instance.expUI.fillAmount = currentExp;
        GameManager.Instance.levelUI.text = level.ToString("N0");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            Dying();
        }

        if (hoverWeapon)
        {
            if (!dialogUI)
            {
                UpdateDialog("Pick up " + hoverWeapon.trueName);
            }
            else
            {
                dialogUI.GetComponent<TextMeshProUGUI>().text = "Pick up " + hoverWeapon.trueName;
            }
        }
        else
        {
            Destroy(dialogUI);
        }
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0
        StartCoroutine(UpdateHealthUI());
    }

    internal IEnumerator UpdateHealthUI()
    {
        float targetFillAmount = currentHealth / maxHealth;
        float startFillAmount = GameManager.Instance.healthUI.fillAmount;
        float duration = 0.25f; // Duration of the transition
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            GameManager.Instance.healthUI.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        GameManager.Instance.healthUI.fillAmount = targetFillAmount;
    }

    internal IEnumerator UpdateManaUI()
    {
        float targetFillAmount = currentMana / maxMana;
        float startFillAmount = GameManager.Instance.manaUI.fillAmount;
        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            GameManager.Instance.manaUI.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            yield return null;
        }
        GameManager.Instance.manaUI.fillAmount = targetFillAmount;
    }


    public void TakeExperince(float earnedExp)
    {
        if (earnedExp + currentExp >= requiredExp)
        {
            level++;
            float expLeft = earnedExp - (requiredExp - currentExp);
            requiredExp = requiredExp + (requiredExp * 0.2f);
            GameManager.Instance.levelUI.text = level.ToString("N0");
            currentExp = 0;
            TakeExperince(expLeft);
        }
        else
        {
            currentExp += earnedExp;
            GameManager.Instance.expUI.fillAmount = currentExp / requiredExp;
        }
    }

    public void UpdateDialog(string dialog)
    {
        dialogUI = GameObject.Instantiate(GameManager.Instance.InteractHintPrefab, GameManager.Instance.CanvasUI.transform);
        dialogUI.transform.SetAsFirstSibling();
        dialogUI.GetComponent<InteractHintUI>().target = dialogPosition;
        dialogUI.GetComponent<TextMeshProUGUI>().text = dialog;
    }

    private void Dying()
    {
        playerControl.animator.SetTrigger("Dying");
        if (currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
        }
    }
    public void Death()
    {
        isDead = true;
        playerControl.animator.SetBool("Death", isDead);
    }

}
