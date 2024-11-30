using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] internal GameObject InteractHintPrefab;
    [SerializeField] internal GameObject CanvasUI;
    [SerializeField] internal GameObject PlayerUI;
    [SerializeField] internal GameObject StatUI;
    [SerializeField] internal GameObject VendorUI;
    [SerializeField] internal GameObject VendorContentUI;
    [SerializeField] internal GameObject VendorOptionPrefab;


    [Header("Player")]
    [SerializeField] internal Image healthUI;
    [SerializeField] internal Image manaUI;
    [SerializeField] internal Image expUI;
    [SerializeField] internal TextMeshProUGUI levelUI;
    [SerializeField] internal TextMeshProUGUI medbagUI;
    [SerializeField] internal TextMeshProUGUI batteryUI;
    [SerializeField] internal LineRenderer weaponTargetingLine;


    [Header("Level")]
    public int killCount;
    public int gold;
    [SerializeField] TextMeshProUGUI killUI;
    [SerializeField] TextMeshProUGUI goldlUI;
    [SerializeField] internal GameObject goldPrefab;
    [SerializeField] internal GameObject expOrbPrefab;

    [Header("Material")]
    public Material flashDamage_Material;

    public enum Pickupable
    {
        medbag,
        battery,
        gold,
        exp
    }
    public static GameManager Instance { get; set; }

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
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            PlayerUI.SetActive(false);
            StatUI.SetActive(false);
            VendorUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateKillCount(int additionalKill = 1)
    {
        killCount += additionalKill;
        killUI.text = killCount.ToString("N0");
    }

    public void UpdateGold(int additionalGold)
    {
        gold += additionalGold;
        goldlUI.text = gold.ToString("N0");
    }

    public Vector3 SetRandomTargetPosition(Vector3 originalPosition, float throwRange)
    {
        Vector3 randomDirection = Random.insideUnitSphere; // Random direction in 3D space
        Vector3 targetPosition = originalPosition + randomDirection.normalized * throwRange;
        return targetPosition;
    }

    public IEnumerator UpdatePosition(GameObject item, Vector3 newPosition, float duration = 0.25f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            try
            {
                item.transform.position = Vector3.Lerp(item.transform.position, newPosition, t);
            }
            catch (System.Exception)
            {
                print("Item has been picked up");
            }
            yield return null;
        }
        if (item) item.transform.position = newPosition;
    }

    public void CloseVenderUI()
    {
        VendorUI.SetActive(false);
        foreach (Transform child in VendorContentUI.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
