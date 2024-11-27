using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal bool isHostile;
    internal float damage;
    internal GameObject target;
    [SerializeField] internal GameObject projectileCollideVFX;
    [SerializeField] float movingSpeed = 3f;
    Rigidbody2D rb;

    private
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // FLY TO LAST DIRECTION OF PLAYER IF IT'S SHOT BY ENEMY
        if (target && isHostile)
        {
            FindTarget();
            StartCoroutine(DestroySelf()); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TRACK TARGET IF IT'S SHOT FROM PLAYER
        if (target && !isHostile)
        {
            FindTarget();
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            Instantiate(projectileCollideVFX, target.transform.position, Quaternion.identity);
            if (isHostile)
            {
                target.GetComponent<Player>().TakeDamage(damage);
            }
            else
            {
                target.GetComponent<Enemy>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void FindTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = direction * movingSpeed;
        transform.up = direction;
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

}
