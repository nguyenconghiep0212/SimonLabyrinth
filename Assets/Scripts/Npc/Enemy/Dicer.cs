using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Dicer : MonoBehaviour
{

    public float maxHealth = 70;
    public float health;
    public float damage = 5;

    Enemy baseClass;
    // Update is called once per frame
    private void Start()
    {
        baseClass = GetComponent<Enemy>();
        health = maxHealth;
    }
    void Update()
    {
    }

    public async void AttackFinish()
    { 
        DealDamage();
        baseClass.animator.SetBool("IsAttackReady", false);
        await Task.Delay(baseClass.attackInterval);
        baseClass.animator.SetBool("IsAttackReady", true);
    }

    private void DealDamage()
    {
        if (baseClass.isPlayerInAttackZone)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(damage);
        } 
    }

    private void OnDrawGizmos()
    {
        // Visualize the overlap area in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, baseClass.attackDistance);
    }
}
