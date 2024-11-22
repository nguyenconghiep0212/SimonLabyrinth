using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isMoving;

    [SerializeField] internal Transform targetTransform;
    [SerializeField] internal float movingSpeed = 3;
    [SerializeField] internal int attackInterval = 5000;
    [SerializeField] internal float attackDistance = 3;
    [SerializeField] internal LayerMask playerLayer;

    internal Rigidbody2D rb;
    internal Animator animator;

    internal bool isPlayerInAttackZone;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsAttackReady", true);

        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        MoveToPlayer();
        CheckPlayerInAttackRange();
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
}
