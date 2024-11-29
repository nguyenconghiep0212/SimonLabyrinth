using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour, EntityInterface
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        InitWeaponOption();
        
    }

    public void InitWeaponOption()
    {
        foreach (Weapon weapon in weapons)
        {
            GameObject weaponOption = Instantiate(GameManager.Instance.VendorOptionPrefab, GameManager.Instance.VendorContentUI.transform);
            weaponOption.GetComponent<VendorOption>().weaponPrefab = weapon;
            weaponOption.GetComponent<VendorOption>().InitOption();
        }
        GameManager.Instance.VendorUI.SetActive(true);
    }
}
