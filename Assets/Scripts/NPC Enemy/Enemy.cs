using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public bool isMoving;
    public float maxHealth = 100;
    public int expDrop = 2;
    public int goldDrop = 5;
    public float health;
    public float damage = 5;

    [SerializeField] internal float movingSpeed = 3;
    [SerializeField] internal int attackInterval = 2;
    [SerializeField] internal LayerMask playerLayer;
    [SerializeField] internal float attackDistance = 3;
    [SerializeField] internal float minDistance = 0;
    [SerializeField] internal Transform barrel;

    [Header("For Range")]
    [SerializeField] internal GameObject projectilePrefab;
    [Header("For Melee")]
    [SerializeField] internal GameObject attackVFX;

    [Header("Flash Damaged")]
    [SerializeField] internal float damageFlashDuration;

    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    internal Rigidbody2D rb;
    internal Animator animator;
    internal Transform targetTransform;
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

            if (hits.Any(hit => hit.collider.CompareTag("Player")))
            {
                isPlayerInAttackZone = true;
                if (animator.GetBool("IsAttackReady") && isPlayerInAttackZone)
                {
                    animator.SetTrigger("Attack"); 
                    if (GetComponent<Range>())
                    {
                        GetComponent<Range>().Attack(); 
                    }
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
            GameManager.Instance.UpdateKillCount();
            isDead = true;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;

            DropLootOnDeath();
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
    public void DropLootOnDeath()
    {
        try
        {
            for (int i = 0; i < goldDrop; i++)
            {
                GameObject item = Instantiate(GameManager.Instance.goldPrefab, transform.position, Quaternion.identity);
                Vector3 newPosition = GameManager.Instance.SetRandomTargetPosition(transform.position, 1f);
                StartCoroutine(GameManager.Instance.UpdatePosition(item, newPosition));
            }

            for (int i = 0; i < expDrop; i++)
            {
                GameObject item = GameObject.Instantiate(GameManager.Instance.expOrbPrefab, transform.position, Quaternion.identity);
                Vector3 newPosition = GameManager.Instance.SetRandomTargetPosition(transform.position, 1);
                StartCoroutine(GameManager.Instance.UpdatePosition(item, newPosition));
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Change color as needed 
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
