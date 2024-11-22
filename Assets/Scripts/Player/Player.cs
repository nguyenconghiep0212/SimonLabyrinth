using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("properties")]
    public float maxHealth = 100;
    public float maxMana = 100;
    public float currentHealth;
    public float currentMana;
    public Image healthUI; 
    public Image manaUI;

    [Header("Level")]
    public int level = 0;
    public int gold;

    private PlayerControl playerControl;
    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        currentHealth = maxHealth;
        currentMana = maxMana;
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

    private IEnumerator UpdateHealthUI()
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
}
