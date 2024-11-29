using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VendorOption : MonoBehaviour
{

    public Weapon weaponPrefab;

    [SerializeField] TextMeshProUGUI trueName;
    [SerializeField] TextMeshProUGUI costUI;
    [SerializeField] Image damageUI;
    [SerializeField] Image fireRateUI;
    [SerializeField] Image lockonsUI;
    [SerializeField] Image rangeUI;
    [SerializeField] Image icon;
     
    public void BuyWeapon()
    {
        if (GameManager.Instance.gold > weaponPrefab.cost)
        { 
            GameManager.Instance.UpdateGold(-weaponPrefab.cost);

            GameObject newWeapon = Instantiate(weaponPrefab.gameObject, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().ChangeWeapon(newWeapon.GetComponent<Weapon>());
        }
    }

    public void InitOption()
    {
        trueName.text = weaponPrefab.trueName;
        costUI.text = weaponPrefab.cost.ToString("N0");
        damageUI.fillAmount = weaponPrefab.damage / 300;
        fireRateUI.fillAmount = weaponPrefab.fireRate / 5;
        rangeUI.fillAmount = weaponPrefab.fireRate / 12;
        lockonsUI.fillAmount = weaponPrefab.fireRate / 3;
        icon.sprite = weaponPrefab.icon;
    }
}
