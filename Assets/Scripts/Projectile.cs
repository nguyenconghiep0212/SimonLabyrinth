using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal float damage;
    internal GameObject target;
    [SerializeField] float movingSpeed = 3f;
    Rigidbody2D rb;

    private
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rb.velocity = direction * movingSpeed;
            transform.up = direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            Instantiate(GameManager.Instance.smokeWxplosionVFX, target.transform.position, Quaternion.identity);
            target.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
