using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isMoving;
    public float maxHealth = 100;
    public float health;

    [SerializeField] internal Transform targetTransform;
    [SerializeField] internal float movingSpeed = 3;
    [SerializeField] internal int attackInterval = 5000;
    [SerializeField] internal float attackDistance = 3;
    [SerializeField] internal LayerMask playerLayer;

    [Header("FLash Damaged")]
    [SerializeField] internal float damageFlashDuration;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    internal Rigidbody2D rb;
    internal Animator animator;

    internal bool isPlayerInAttackZone;
    internal bool isDead = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAttackReady", true);

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;

        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            MoveToPlayer();
            CheckPlayerInAttackRange();
        }
    }

    public void MoveToPlayer()
    {
        float distance = Vector3.Distance(targetTransform.position, transform.position);
        if (distance > attackDistance)
        {
            isMoving = true;

            Vector3 direction = (targetTransform.position - transform.position).normalized;
            rb.velocity = direction * movingSpeed;

            animator.SetBool("IsMoving", isMoving);
            if (rb.velocity.x > 0)
            {
                animator.SetFloat("MoveX", 1);
            }
            else if (rb.velocity.x < 0)
            {
                animator.SetFloat("MoveX", -1);
            }
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", isMoving);
        }
    }


    private void CheckPlayerInAttackRange()
    {
        float distance = Vector3.Distance(targetTransform.position, transform.position);

        if (distance <= attackDistance)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackDistance, Vector2.zero, 0f, playerLayer);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        isPlayerInAttackZone = true;
                        if (animator.GetBool("IsAttackReady") && isPlayerInAttackZone) animator.SetTrigger("Attack");
                    }
                    else
                    {
                        isPlayerInAttackZone = false;
                    }
                }
                else
                {
                    isPlayerInAttackZone = false;
                }
            }
        }
        else
        {
            isPlayerInAttackZone = false;
        }
    }

    internal void TakeDamage(float damageTaken)
    {
        Flash();
        health -= damageTaken;
        if (health <= 0)
        {
            GameManager.Instance.killCount++;
            GameManager.Instance.UpdateKillCount();
            isDead = true;
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Death");
        }
    }

    private void Flash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(DamageFlashRoutine());
    }
    private IEnumerator DamageFlashRoutine()
    {
        spriteRenderer.material = GameManager.Instance.flashDamage_Material;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
