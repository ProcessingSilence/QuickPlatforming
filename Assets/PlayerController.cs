// MAIN MECHANIC: A quick and consistent jump that is NOT affected by how long space is held.

/*                                            *** NOTE ***
 * The jump + movement input and velocity are kept separate, with input in Update() and velocity in FixedUpdate().
 * The reason for this is because putting velocity changes in Update() can cause inconsistencies due to physics updates.
 
 * I just found out about this after 2 hours of debugging, and I DO NOT want you to have the same problem.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Platform mask when falling, no layer mask when jumping.
    [SerializeField]private LayerMask platformLayerMask;
    // "emptyLayerMask is never assigned" is a lie, do not delete it or it borks the one-way platforms
    private LayerMask emptyLayerMask;
    
    // Jump
    public float jumpVel;
    public float gravityWeight = 2f;
    private Vector2 jumpVec;
    
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
        //YVelCheck();
        MovementInput();
        rb.velocity = new Vector2(movVec, rb.velocity.y);
    }

    // Gives player upward velocity only once, so it can be used in Update() safely.
    void JumpingInput()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.W))
        {        
            rb.velocity = Vector2.up * jumpVel;
        }         
    }

    void Gravity()
    {
        rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityWeight - 1);
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
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(0,0,0);
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,Vector2.down, .7f, platformLayerMask);
        /*
        if (raycastHit.collider != null)
        {
            Debug.Log("Grounded");
        }
        else
        {
            Debug.Log("Midair");
        }
        */
        return raycastHit.collider != null;
    }

    // yVel < 0: Platform layer mask.
    // yVel > 0: Empty layer mask.
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
