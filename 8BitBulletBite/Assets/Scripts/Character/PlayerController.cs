using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public int jumpVelocity = 8;
    public int maxMovementSpeed = 20;
    public int acceleration = 50;
    public int gravityMultiplier = 3;

    public int dashVelocity = 50;
    public float dashTime = 0.5f;
    private bool isInMotion = false;
    private bool dashCD = false;

    private float moveDirection;
    private bool facingRight = true;
    [SerializeField]
    private Transform characterBody;

    private bool isGrounded = false;
    public LayerMask whatIsGround;
    [SerializeField]
    private Transform groundChecker;
    private float collisionRadius = 0.5f;

    public float wallJumpVelocity = 30f;
    public float wallJumpDuration = 0.5f;
    private bool wallRide = false;
    public LayerMask whatIsWall;
    [SerializeField]
    public Transform wallChecker;

    private bool fallJump = false;
    private Rigidbody2D rigidBody;
    [SyncVar(hook = "OnUpdateScale")]
    private float scale;

    [SerializeField]
    private AudioClip jumpClip;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isLocalPlayer) {
            return;
        }

        if (!isInMotion) {
            CollisionChecking();
            MaxSpeedManager();
            GravityPull();
            SlowDown();
            Movement();
        }
    }

    private void CollisionChecking()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, collisionRadius, whatIsGround);
        wallRide = Physics2D.OverlapCircle(wallChecker.position, collisionRadius, whatIsGround);
        if (isGrounded) {
            fallJump = true;
        }
    }

    private void MaxSpeedManager()
    {
        rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -maxMovementSpeed, maxMovementSpeed), rigidBody.velocity.y);
    }

    private void GravityPull()
    {
        if (wallRide ||isGrounded) {
            rigidBody.gravityScale = 3f;
        } else if (rigidBody.velocity.y < 0) {
            rigidBody.gravityScale = gravityMultiplier;
        } else if (rigidBody.velocity.y > 0 && !Input.GetButton("Jump")) {
            rigidBody.gravityScale = gravityMultiplier;
        } else if (rigidBody.velocity.y > 0) {
            rigidBody.gravityScale = 3f;
        } else {
            rigidBody.gravityScale = 3;
        }
    }

    private void SlowDown()
    {
        if (isGrounded && moveDirection == 0) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x * 0.5f, rigidBody.velocity.y);
        }
    }

    private void Movement()
    {
        moveDirection = Input.GetAxisRaw("Horizontal");
        Move();
        if (Input.GetButtonDown("Jump")) {
            Jump();
        }
        if (Input.GetButtonDown("Dash") && !dashCD) {
            Dash();
        }
    }

    private void Move()
    {
        if (isGrounded) {
            rigidBody.AddForce(new Vector2(moveDirection, 0) * acceleration);
        } else {
            rigidBody.AddForce(new Vector2(moveDirection, 0) * (acceleration - 10));
        }

        if (moveDirection > 0 && !facingRight) {
            Flip();
        } else if (moveDirection < 0 && facingRight) {
            Flip();
        }
    }


    private void Flip()
    {
        facingRight = !facingRight;
        characterBody.transform.localScale = new Vector2(characterBody.transform.localScale.x * -1, characterBody.transform.localScale.y);
        CmdScale(characterBody.transform.localScale.x);
    }

    [Command]
    private void CmdScale(float newScale)
    {
        scale = newScale;
    }

    private void OnUpdateScale(float newScale)
    {
        scale = newScale;
        characterBody.transform.localScale = new Vector2(scale, characterBody.transform.localScale.y);
    }

    private void Jump()
    {
        if (isGrounded) {
            RegularJump();
        } else if (wallRide) {
            WallJump();
        } else if (fallJump) {
            RegularJump();
        }
    }

    private void RegularJump()
    {
        AudioSource.PlayClipAtPoint(jumpClip,transform.position);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        fallJump = false;
    }

    private void WallJump()
    {
        isInMotion = true;
        rigidBody.velocity = new Vector2(0, 0);
        if (facingRight) {
            Flip();
            rigidBody.AddForce(new Vector2(-wallJumpVelocity, wallJumpVelocity - 5), ForceMode2D.Impulse);
        } else {
            Flip();
            rigidBody.AddForce(new Vector2(wallJumpVelocity, wallJumpVelocity - 5), ForceMode2D.Impulse);
        }
        StartCoroutine(WallJumping(wallJumpDuration));
    }

    private IEnumerator WallJumping(float walljumpTime)
    {
        yield return new WaitForSeconds(wallJumpDuration);
        isInMotion = false;
    }

    private void FallJump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
        rigidBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        fallJump = false;
    }

    private void Dash()
    {
        dashCD = true;
        isInMotion = true;
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        if (facingRight) {
            rigidBody.AddForce(Vector2.right * dashVelocity, ForceMode2D.Impulse);
        } else {
            rigidBody.AddForce(Vector2.left * dashVelocity, ForceMode2D.Impulse);
        }
        StartCoroutine(Dashing(dashTime));
    }

    private IEnumerator Dashing(float dashTime)
    {
        yield return new WaitForSeconds(dashTime);
        isInMotion = false;
        yield return new WaitForSeconds(1);
        dashCD = false;
    }
}