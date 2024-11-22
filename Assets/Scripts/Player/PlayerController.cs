using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float movingSpeed;
    public bool isMoving;

    private Vector2 input;

    private Animator animator;
    private Rigidbody2D rb;

    public LayerMask solidObjectLayer;
    public LayerMask npcLayer;

    private Player playerProperties;
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

    // Update is called once per frame 
    void FixedUpdate()
    {
        Move();
        Interact();
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var facingDir = new Vector3(animator.GetFloat("MoveX"), animator.GetFloat("MoveY"));
            var interactPos = transform.position + facingDir; 

            var collider = Physics2D.OverlapCircle(interactPos, 0.2f, npcLayer);
            if (collider)
            {
                collider.GetComponent<NpcInteractable>()?.Interact();
            }
        }
    }

    private void Move()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);

        Vector3 targetPosition = transform.position;
        targetPosition += (Vector3)input;

        if (IsWalkable(targetPosition))
        {
            rb.velocity = new Vector2(input.x * movingSpeed, input.y * movingSpeed);
            isMoving = true;
         }

        if (input == Vector2.zero)
        {
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
    }

 
    private bool IsWalkable(Vector3 targetPosition)
    {
        if (Physics2D.OverlapCircle(targetPosition, 0.1f, solidObjectLayer | npcLayer) != null)
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        // Visualize the overlap area in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}
