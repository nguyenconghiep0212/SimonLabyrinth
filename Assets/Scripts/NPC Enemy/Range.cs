using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Range : MonoBehaviour
{
    Enemy baseClass;

     void Awake()
    {
        baseClass = GetComponent<Enemy>();
    } 

    public void Attack()
    {
        GameObject projectile = Instantiate(baseClass.projectilePrefab, baseClass.barrel.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().target = baseClass.targetTransform.gameObject;
        projectile.GetComponent<Projectile>().isHostile = true;
        projectile.GetComponent<Projectile>().damage = baseClass.damage;
        baseClass.animator.SetBool("IsAttackReady", false);
        StartCoroutine(resetAttack());
    }

    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(baseClass.attackInterval);
        baseClass.animator.SetBool("IsAttackReady", true);

    }
}
