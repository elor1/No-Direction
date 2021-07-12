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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Direction.None;

        canJump = true;
        canMoveRight = true;
        canMoveLeft = true;
        canStop = true;
        canWallJump = true;

        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Update input
        if (Input.GetKey(KeyCode.D) && canMoveRight)
        {
            canMoveRight = false;
            currentDirection = Direction.Right;
            transform.localScale = new Vector3(2.0f, transform.localScale.y, transform.localScale.z);
        }
        else if (Input.GetKey(KeyCode.A) && canMoveLeft)
        {
            canMoveLeft = false;
            currentDirection = Direction.Left;
            transform.localScale = new Vector3(-2.0f, transform.localScale.y, transform.localScale.z);
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
}
