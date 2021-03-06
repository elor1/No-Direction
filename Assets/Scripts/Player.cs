using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField] bool canJump, canMoveRight, canMoveLeft, canStop;
    private bool canWallJump, isJumping, isGrounded;

    Vector3 velocity = Vector3.zero;

    private float wallJumpCooldown;

    private Animator anim;

    [SerializeField] private ControlImages WImage,AImage, SImage, DImage;

    [SerializeField] private TextMeshProUGUI helpText;

    [SerializeField] private AudioClip[] sfx;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Direction.None;

        //canJump = true;
        //canMoveRight = true;
        //canMoveLeft = true;
        //canStop = true;

        canWallJump = false;
        isJumping = false;
        isGrounded = true;

        wallJumpCooldown = 0.0f;

        anim = GetComponent<Animator>();

        if (!canJump)
        {
            WImage.Disable();
        }
        if (!canMoveLeft)
        {
            AImage.Disable();
        }
        if (!canMoveRight)
        {
            DImage.Disable();
        }
        if (!canStop)
        {
            SImage.Disable();
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.currentState == GameManager.GameState.Playing)
        {
            //Update input
            if (Input.GetKey(KeyCode.R))
            {
                GameManager.currentState = GameManager.GameState.Intro;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }


            if (Input.GetKey(KeyCode.D) && canMoveRight)
            {
                audioSource.Play();
                canMoveRight = false;
                DImage.Disable();
                FlipRight();
            }
            else if (Input.GetKey(KeyCode.A) && canMoveLeft)
            {
                audioSource.Play();
                canMoveLeft = false;
                AImage.Disable();
                FlipLeft();
            }
            else if (Input.GetKey(KeyCode.S) && canStop)
            {
                audioSource.Play();
                canStop = false;
                SImage.Disable();
                currentDirection = Direction.None;
            }
            else if (Input.GetKey(KeyCode.W) && canJump && isGrounded)
            {
                audioSource.Play();
                canJump = false;
                WImage.Disable();
                isJumping = true;
            }

            if (Input.GetKey(KeyCode.Space) && canWallJump && wallJumpCooldown >= 0.3f)
            {
                audioSource.Play();
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

        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            canWallJump = false;
        }

        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Spikes")
        {
            audioSource.clip = sfx[1];
            audioSource.Play();
            StopMovement();
            helpText.text = "R to restart";
            Jump();

            //Add rotation
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(100.0f);
            CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
            capsuleCollider.enabled = false;
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

    public void StopMovement()
    {
        currentDirection = Direction.None;
        canMoveRight = false;
        canMoveLeft = false;
        canStop = false;
        canJump = false;
    }
}
