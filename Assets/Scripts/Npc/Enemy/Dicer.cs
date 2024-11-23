using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dicer : MonoBehaviour
{


    public float damage = 5;

    Enemy baseClass;
    // Update is called once per frame
    private void Start()
    {
        baseClass = GetComponent<Enemy>();
    }
    void Update()
    {
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
        catch (System.Exception error)
        {
            Debug.Log(gameObject.name + " has died");
        }
    }

    private void DealDamage()
    {
        if (baseClass.isPlayerInAttackZone)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        }
    }

}
