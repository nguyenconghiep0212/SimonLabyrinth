using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Melee : MonoBehaviour
{

    Enemy baseClass;
     void Awake()
    {
        baseClass = GetComponent<Enemy>();
    } 

    public async void AttackFinish()
    {
        DealDamage();
        baseClass.animator.SetBool("IsAttackReady", false);
        await Task.Delay(baseClass.attackInterval);
        try
        {
            baseClass.animator.SetBool("IsAttackReady", true);
        }
        catch (System.Exception e)
        {
            Debug.Log("GameObject error:" + e);
        }
    }

    private void DealDamage()
    {
        if (baseClass.isPlayerInAttackZone)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(baseClass.damage);
        }
    }
}
