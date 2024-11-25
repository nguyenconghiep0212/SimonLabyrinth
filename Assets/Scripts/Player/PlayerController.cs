using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float movingSpeed;
    public bool isMoving;

    private Vector2 input;

    private Animator animator;
    private Rigidbody2D rb;

    public LayerMask entityLayer;

    private Player playerProperties;
    float faceDirection = 0;
    float interactionRange = 1f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerProperties = GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        InteractionKey();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x == 0)
        {
            if (faceDirection < 0) faceDirection = -0.01f;
            if (faceDirection > 0) faceDirection = 0.01f;
        }
        else
        {
            faceDirection = input.x;
        }

        RotateWeapon(faceDirection);
        animator.SetFloat("MoveX", faceDirection);
        animator.SetFloat("MoveY", input.y);

        rb.velocity = new Vector2(input.x * movingSpeed, input.y * movingSpeed);
        isMoving = rb.velocity != Vector2.zero;

        if (input == Vector2.zero)
        {
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
    }

    private void RotateWeapon(float faceDirection)
    {
        foreach (Transform child in GameManager.Instance.playerWeaponPosition)
        {
            Animator animator = child.GetComponent<Animator>();
            animator.SetFloat("MoveX", faceDirection);
        }
    }

    private void InteractionKey()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal();
        }
    }
    private void Interact()
    {
        //Vector3 facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
        //Vector3 interactPos = transform.position + facingDir; 

        Collider2D entityCollider = Physics2D.OverlapCircle(transform.position, interactionRange, entityLayer);
        if (entityCollider)
        {
            entityCollider.GetComponent<EntityInterface>()?.Interact();
        }
    }

    public void Heal()
    {

        if (playerProperties.currentHealth < playerProperties.maxHealth && playerProperties.medbag > 0)
        {
            playerProperties.medbag--;
            playerProperties.medbagUI.text = playerProperties.medbag.ToString();
            if (playerProperties.currentHealth + (playerProperties.maxHealth * 0.3f) <= playerProperties.maxHealth)
            {
                playerProperties.currentHealth = playerProperties.currentHealth + (playerProperties.maxHealth * 0.3f);
            }
            else
            {
                playerProperties.currentHealth = playerProperties.maxHealth;
            }
            StartCoroutine(playerProperties.UpdateHealthUI());
        }
    }
    public void Recharge()
    {
        if (playerProperties.currentMana < playerProperties.maxMana && playerProperties.battery > 0)
        {
            playerProperties.battery--;
            playerProperties.batteryUI.text = playerProperties.battery.ToString();
            if (playerProperties.currentMana + (playerProperties.maxMana * 0.5f) <= playerProperties.maxMana)
            {
                playerProperties.currentMana = playerProperties.currentMana + (playerProperties.maxMana * 0.5f);
            }
            else
            {
                playerProperties.currentMana = playerProperties.maxMana;
            }
            StartCoroutine(playerProperties.UpdateHealthUI());
        }
    }
}
