using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    internal bool inCombat;

    [Header("UI")]
    public Image healthUI;
    public Image manaUI;
    public Image expUI;
    public TextMeshProUGUI levelUI;
    public TextMeshProUGUI medbagUI;
    public TextMeshProUGUI batteryUI;

    private PlayerControl playerControl;
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentExp = 0;
        expUI.fillAmount = currentExp;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

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
        float startFillAmount = healthUI.fillAmount;
        float duration = 0.25f; // Duration of the transition
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            healthUI.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        healthUI.fillAmount = targetFillAmount;
    }

    internal IEnumerator UpdateManaUI()
    {
        float targetFillAmount = currentMana / maxMana;
        float startFillAmount = manaUI.fillAmount;
        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            manaUI.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, t);
            yield return null;
        } 
        manaUI.fillAmount = targetFillAmount;
    }


    public void TakeExperince(float earnedExp)
    {
        if (earnedExp + currentExp >= requiredExp)
        {
            level++;
            float expLeft = earnedExp - (requiredExp - currentExp);
            requiredExp = requiredExp + (requiredExp * 0.2f);
            levelUI.text = level.ToString("N0");
            currentExp = 0;
            TakeExperince(expLeft);
        }
        else
        {
            currentExp += earnedExp;
            expUI.fillAmount = currentExp / requiredExp;
        }
    }

}
