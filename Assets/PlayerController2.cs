// MAIN MECHANIC: A quick and consistent jump (plus multi-jump) that is NOT affected by how long space is held.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField]private LayerMask platformLayerMask;
    
    // Jump
    public float jumpVel;
    private Vector2 jumpVec;
        // Tells how many midair jumps are left. Edit this as much as you want.
    public int multiJumpLimit;
    public int currentJumpsLeft;
    public float maxJumpHeight;
    private Vector2 currentJumpPosition;
    private bool numeratorRunning;
    private int iJumped;
    float fJumpPressedRemember = 0;
    float fJumpPressedRememberTime = 0.2f;
    
    // Movement 
    public float moveSpeed;
    private float movVec;
    
    // Multiplies horizontal velocity to determine direction in FixedUpdate().
    private int inputDirection;
    
    // Components
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
   
    // Fall Death
    public float fallDeathPos;
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        currentJumpsLeft = multiJumpLimit;
    }

    private void FixedUpdate()
    {
        FallDeath();
        MovementOutput();

        FallVelocityCheck();
    }

    private void Update()
    {
        JumpingInput();
        Gravity();
        MovementInput();
        rb.velocity = new Vector2(movVec, rb.velocity.y);
    }

    // Gives player upward velocity only once, so it can be used in Update() safely.
    // Can only jump input when velocity <= 0 so player cannot jump while they're in the middle of one-way platforms.
    void JumpingInput()
    {
        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.W))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
        
        // Uses "GetKey" so that the jumping is not sensitive
        if ((IsGrounded() && rb.velocity.y <= 0 && (fJumpPressedRemember > 0)))
        {
            iJumped = 1;
            currentJumpPosition = new Vector2(transform.position.x, transform.position.y + maxJumpHeight);
            rb.velocity = Vector2.up * jumpVel;
            currentJumpsLeft = multiJumpLimit;
        }

        if (!IsGrounded() && currentJumpsLeft > 0 && Input.GetKeyDown(KeyCode.W))
        {
            iJumped = 1;
            currentJumpPosition = new Vector2(transform.position.x, transform.position.y + maxJumpHeight);
            currentJumpsLeft -= 1;
            rb.velocity = Vector2.up * jumpVel;
        }


        if (IsGrounded() && iJumped != 1)
        {
            iJumped = 0;
        }
    }

    void Gravity()
    {
        //rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityWeight - 1);
        if (!IsGrounded() && iJumped == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpVel);
        }
        
    }

    void MovementInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            inputDirection = 1;
        }     
        else if (Input.GetKey(KeyCode.A))
        {
            inputDirection = -1;
        }    
        else
        {
            inputDirection = 0;
        }
    }

    void MovementOutput()
    {
        movVec = moveSpeed * inputDirection * Time.fixedDeltaTime;
    }
    
    // Pseudo-respawns player after falling to determined y value
    void FallDeath()
    {
        if (transform.position.y <= fallDeathPos)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            transform.position = new Vector3(0,0,0);
        }
    }

    void FallVelocityCheck()
    {
        if (!IsGrounded())
        {
            if (transform.position.y > currentJumpPosition.y && rb.velocity.y > 0)
            {
                iJumped = 2;
                var tempVel = rb.velocity;
                rb.velocity = new Vector2(tempVel.x, -jumpVel);
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,Vector2.down, .2f, platformLayerMask);
        return raycastHit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform"))
        {
            fJumpPressedRemember = 0;
        }
    }
}
