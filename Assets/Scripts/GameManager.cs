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

    [Header("Material")]
    public Material flashDamage_Material;

    public enum UsableType
    {
        medbag,
        battery
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
        killUI.text = killCount.ToString("N0");
    }

    public void UpdateGold()
    {
        goldlUI.text = gold.ToString("N0");
    }
}
