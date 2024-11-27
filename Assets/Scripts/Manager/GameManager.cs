using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] internal GameObject InteractHintUI;
    [SerializeField] internal GameObject CanvasUI;

    [Header("Player")]
    [SerializeField] internal Transform playerWeaponPosition;
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
            //DontDestroyOnLoad(Instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateKillCount()
    {
        killCount++;
        killUI.text = killCount.ToString("N0");
    }

    public void UpdateGold(int goldCount)
    {
        gold += goldCount;
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

    
}
