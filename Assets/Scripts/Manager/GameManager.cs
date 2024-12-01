using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Static instance of the GameManager
    private static GameManager instance; 
    // Property to access the instance
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
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

    [Header("UI")]
    [SerializeField] internal GameObject InteractHintPrefab;
    [SerializeField] internal GameObject CanvasUI;
    [SerializeField] internal GameObject PlayerUI;
    [SerializeField] internal GameObject StatUI;
    [SerializeField] internal GameObject VendorUI;
    [SerializeField] internal GameObject VendorContentUI;
    [SerializeField] internal GameObject VendorOptionPrefab;

    [Header("Playable Characters")]
    [SerializeField] internal AnimatorController HeinrichVonKropp_Animator;
    [SerializeField] internal AnimatorController ChrisTirtaKohler_Animator;
    [SerializeField] internal AnimatorController IreneeCyrilleLoritz_Animator;
    [SerializeField] internal Sprite HeinrichVonKropp_Idle;
    [SerializeField] internal Sprite ChrisTirtaKohler_Idle;
    [SerializeField] internal Sprite IreneeCyrilleLoritz_Idle;
    [SerializeField] internal Sprite HeinrichVonKropp_Icon;
    [SerializeField] internal Sprite ChrisTirtaKohler_Icon;
    [SerializeField] internal Sprite IreneeCyrilleLoritz_Icon;


    [Header("Player")]
    [SerializeField] internal PlayableCharacter selectedCharacter;
    [SerializeField] internal Image healthUI;
    [SerializeField] internal Image manaUI;
    [SerializeField] internal Image expUI;
    [SerializeField] internal TextMeshProUGUI levelUI;
    [SerializeField] internal TextMeshProUGUI medbagUI;
    [SerializeField] internal TextMeshProUGUI batteryUI;
    [SerializeField] internal LineRenderer weaponTargetingLine;
    [SerializeField] internal List<Weapon> totalWeaponList;


    [Header("Level")]
    public int killCount;
    public int gold;
    [SerializeField] TextMeshProUGUI killUI;
    [SerializeField] TextMeshProUGUI goldlUI;
    [SerializeField] internal GameObject goldPrefab;
    [SerializeField] internal GameObject expOrbPrefab;

    [Header("Material")]
    public Material flashDamage_Material;

    public enum WeaponEnum
    {
        StandardRifle,
        SMG,
        BattleRifle,
        SmartRifle,
        LaserRifle,
        SniperRifle,
        PlasmaBlaster,
        RocketLauncher
    }
    public enum PlayableCharacter
    {
        HeinrichVonKropp,
        ChrisTirtaKohler,
        IreneeCyrilleLoritz
    }
    public enum Pickupable
    {
        medbag,
        battery,
        gold,
        exp
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

    public void ApplySelectedCharacterToPlayer(PlayableCharacter character)
    {
        selectedCharacter = character;
        SaveManager.Instance.LoadGame();
    }
    public void InitPlayer()
    {
        switch (selectedCharacter)
        {
            case PlayableCharacter.HeinrichVonKropp:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().animator.runtimeAnimatorController = HeinrichVonKropp_Animator;
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sprite = HeinrichVonKropp_Idle;
                break;
            case PlayableCharacter.ChrisTirtaKohler:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().animator.runtimeAnimatorController = ChrisTirtaKohler_Animator;
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sprite = ChrisTirtaKohler_Idle;
                break;
            case PlayableCharacter.IreneeCyrilleLoritz:
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().animator.runtimeAnimatorController = IreneeCyrilleLoritz_Animator;
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sprite = IreneeCyrilleLoritz_Idle;
                break;
        }
        PlayerUI.transform.GetChild(0).GetComponent<Image>().sprite = GetCharacterIcon(selectedCharacter);
    }

    public Sprite GetCharacterIcon(PlayableCharacter character)
    {
        switch (character)
        {
            case PlayableCharacter.HeinrichVonKropp:
                return HeinrichVonKropp_Icon;
            case PlayableCharacter.ChrisTirtaKohler:
                return ChrisTirtaKohler_Icon;
            case PlayableCharacter.IreneeCyrilleLoritz:
                return IreneeCyrilleLoritz_Icon;
            default:
                return null;
        }
    }
    public Weapon GetPlayerSavedWeapon(int? weaponId)
    {
        if (weaponId != null)
        {
            Weapon prefab = totalWeaponList.First(item => (int)item.id == weaponId);
            GameObject finalWeapon = Instantiate(prefab.gameObject); 
            return finalWeapon.GetComponent<Weapon>();
        }
        return null;
    }
}
