using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

// Uses a quick jump that is NOT affected by how long space is held.
public class PlayerController : MonoBehaviour
{
    // Platform mask when falling, no layer mask when jumping.
    // ("emptyLayerMask is never assigned" is a lie, don't delete it)
    [SerializeField]private LayerMask platformLayerMask;
    private LayerMask emptyLayerMask;
    
    // Jump
    public float jumpVel;
    public float lowJumpMult = 2f;
    private Vector2 jumpVec;
    
    // Movement 
    public float moveSpeed;
    private float movVec;
    private int movementDirection;
    
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
        FallDeath();
        MovementOutput();
        Gravity();
    }

    private void Update()
    {
        JumpingInput();
        YVelCheck();
        MovementInput();
        rb.velocity = new Vector2(movVec, rb.velocity.y);
    }

    
    // Activates jumping in FixedUpdate(). Putting jumping in void Update() leads to inconsistent jump heights
    // due to messing with physics.
    void JumpingInput()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.W))
        {        
            rb.velocity = Vector2.up * jumpVel;
        }         
    }

    void Gravity()
    {
        rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1);
    }

    void MovementInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection = 1;
        }     
        else if (Input.GetKey(KeyCode.A))
        {
            movementDirection = -1;
        }    
        else
        {
            movementDirection = 0;
        }
    }

    void MovementOutput()
    {
        movVec = moveSpeed * movementDirection * Time.fixedDeltaTime;
    }

    void FallDeath()
    {
        // Pseudo-Respawn
        if (transform.position.y <= fallDeathPos)
        {
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(0,0,0);
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

    void YVelCheck()
    {
        if (rb.velocity.y <= 0)
        {
            platformLayerMask = 7 << 8;
        }
        else
        {
            platformLayerMask = emptyLayerMask;
        }
    }
}
