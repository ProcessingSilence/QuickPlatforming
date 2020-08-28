using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

// Uses a quick jump that is NOT affected by how long space is held.
public class PlayerController : MonoBehaviour
{
    [SerializeField]private LayerMask platformLayerMask;
    
    // Jump
    public float jumpVel;
    public float lowJumpMult = 2f;
    private Vector2 jumpVec;
    
    // Movement 
    public float moveSpeed;
    private float movVec;
    private int isMoving;
    
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
       
    // Fall Death
    public float fallDeathPos;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.y <= fallDeathPos)
            transform.position = new Vector3(0,0,0);
    }


    private void Update()
    {
        Jumping();
        Movement();
        rb.velocity = new Vector2(movVec, rb.velocity.y);
    }


    void Jumping()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.W))
            rb.velocity = Vector2.up * jumpVel;         
        else if (!IsGrounded())
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.deltaTime;        
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (isMoving != 1)
            {
                isMoving = 1;
                movVec = moveSpeed / 2;
            }

            movVec = moveSpeed * Time.fixedDeltaTime;
        }     
        else if (Input.GetKey(KeyCode.A))
        {
            if (isMoving != 2)
            {
                isMoving = 1;
                movVec = moveSpeed / 2;
            }
            movVec = -moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            isMoving = 0;
            movVec = 0;
        }

    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,Vector2.down, .7f, platformLayerMask);
        if (raycastHit.collider != null)
        {
            Debug.Log("Grounded");
        }
        else
        {
            Debug.Log("Midair");
        }
        return raycastHit.collider != null;
    }

}
