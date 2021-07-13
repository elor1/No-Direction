using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum Direction
    {
        None,
        Left,
        Right
    };

    private Rigidbody2D rb;
    private Direction currentDirection;
    [SerializeField] private float movementSpeed = 50.0f;
    [SerializeField] private float jumpHeight = 550.0f;

    private bool canJump, canMoveRight, canMoveLeft, canStop, canWallJump, isJumping;

    Vector3 velocity = Vector3.zero;

    private float wallJumpCooldown;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Direction.None;

        canJump = true;
        canMoveRight = true;
        canMoveLeft = true;
        canStop = true;

        canWallJump = false;
        isJumping = false;

        wallJumpCooldown = 0.0f;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update input
        if (Input.GetKey(KeyCode.D) && canMoveRight)
        {
            canMoveRight = false;
            FlipRight();
        }
        else if (Input.GetKey(KeyCode.A) && canMoveLeft)
        {
            canMoveLeft = false;
            FlipLeft();
        }
        else if (Input.GetKey(KeyCode.S) && canStop)
        {
            canStop = false;
            currentDirection = Direction.None;
        }
        else if (Input.GetKey(KeyCode.W) && canJump)
        {
            canJump = false;
            isJumping = true;
        }

        if (Input.GetKey(KeyCode.Space) && canWallJump && wallJumpCooldown >= 0.3f)
        {
            isJumping = true;
            if (currentDirection == Direction.Right)
            {
                FlipLeft();
            }
            else if (currentDirection == Direction.Left)
            {
                FlipRight();
            }

            wallJumpCooldown = 0.0f;
        }

        wallJumpCooldown += Time.deltaTime;

        UpdateAnimations(); 
    }

    void FixedUpdate()
    {
        
        if (currentDirection == Direction.Left)
        {
            Move(-movementSpeed * Time.deltaTime);
        }
        else if (currentDirection == Direction.Right)
        {
            Move(movementSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        if (isJumping)
        {
            Jump();
        }
    }

    void FlipRight()
    {
        currentDirection = Direction.Right;
        transform.localScale = new Vector3(2.0f, transform.localScale.y, transform.localScale.z);
    }

    void FlipLeft()
    {
        currentDirection = Direction.Left;
        transform.localScale = new Vector3(-2.0f, transform.localScale.y, transform.localScale.z);
    }

    void Move(float amount)
    {
        Vector3 targetVelocity = new Vector2(amount * 10.0f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
    }

    void Jump()
    {
        isJumping = false;
        rb.AddForce(new Vector2(0.0f,  jumpHeight));
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            canWallJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            canWallJump = false;
        }
    }

    void UpdateAnimations()
    {
        if (currentDirection == Direction.None)
        {
            anim.SetBool("IsRunning", false);
        }
        else
        {
            anim.SetBool("IsRunning", true);
        }

        anim.SetBool("IsJumping", isJumping);
    }
}
