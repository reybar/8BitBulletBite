using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    
    public int jumpVelocity=8;
    public int movementSpeed=20;
    public int acceleration = 50;
    public int gravityMultiplier = 3;

    public int dashVelocity = 50;
    public float dashTime = 0.5f;
    private bool isInMotion = false;
    private bool dashCD = false;

    private float moveDir;
    private bool facingRight = true;
    private Transform characterBody;

    private bool isGrounded = false;
    public LayerMask whatIsGround;
    private Transform groundChecker;
    private float collisionRadius = 0.2f;

    public float wallJumpVelocity = 30f;
    public float wallJumpTime = 0.5f;
    private bool wallRide = false;
    public LayerMask whatIsWall;
    public Transform wallChecker;

    private bool extraJump = false;
    private Rigidbody2D rBody;
    [SyncVar(hook = "OnUpdateScale")]
    private float scale;
    


    // Use this for initialization
    void Start() {
        rBody = GetComponent<Rigidbody2D>();
        characterBody = transform.Find("Body");
        groundChecker = characterBody.transform.Find("Ground Checker");
        wallChecker = characterBody.transform.Find("Wall Checker");        
    }

    // Update is called once per frame
    void Update() {

        
        if(!isLocalPlayer) {
            return;
        }
        
        if(isGrounded && moveDir == 0 && !isInMotion) {
            rBody.velocity = new Vector2(rBody.velocity.x*0.5f, rBody.velocity.y);
        }
        

        CollisionChecking();
        
        if(!isInMotion) {
            Jump();
            Dashing();
            WallJumping();
        }
    }

    private void FixedUpdate() {

        if(!isLocalPlayer) {
            return;
        }

        if(!isInMotion) {

            //max speed
            rBody.velocity = new Vector2(Mathf.Clamp(rBody.velocity.x, -movementSpeed, movementSpeed), rBody.velocity.y);

            Run();
            Falling();
        }     
    }



    void Flip() {
        facingRight = !facingRight;
        characterBody.transform.localScale = new Vector2(characterBody.transform.localScale.x *-1,characterBody.transform.localScale.y);

        CmdScale(characterBody.transform.localScale.x);
        }


    [Command]
    void CmdScale(float newScale) {
        scale = newScale;
    }

    void OnUpdateScale(float newScale) {
        scale = newScale;
        characterBody.transform.localScale = new Vector2(scale, characterBody.transform.localScale.y);
    }

    void Run() {



        moveDir = Input.GetAxis("Horizontal");

        if(isGrounded) {
            rBody.AddForce(new Vector2(moveDir, 0) * acceleration);
        }
        else {
            rBody.AddForce(new Vector2(moveDir, 0) * (acceleration-10));
        }


        if(moveDir > 0 && !facingRight) {
            Flip();
        }
        else if(moveDir < 0 && facingRight) {
            Flip();
        }
        
        
    }

    void Jump() {
        if(Input.GetButtonDown("Jump") && !wallRide) {
            if(isGrounded) {
                rBody.velocity = new Vector2(rBody.velocity.x, 0);
                rBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
            }
            else if(!isGrounded && extraJump) {
                rBody.velocity = new Vector2(rBody.velocity.x, 0);
                rBody.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
                extraJump =false;
            }
        }
    }

    void WallJumping() {
        if(Input.GetButtonDown("Jump") && wallRide) {
            isInMotion = true;
            rBody.velocity = new Vector2(0, 0);
            if(facingRight) {
                Flip();
                rBody.AddForce(new Vector2(-wallJumpVelocity, wallJumpVelocity-5), ForceMode2D.Impulse);
            }
            else {
                Flip();
                rBody.AddForce(new Vector2(wallJumpVelocity, wallJumpVelocity-5), ForceMode2D.Impulse);
            }
            StartCoroutine(WallJump(wallJumpTime));
        }
    }

    IEnumerator WallJump(float walljumpTime) {
        yield return new WaitForSeconds(wallJumpTime);
        isInMotion = false;
    }

    void Falling() {
        if(wallRide) {
            
            rBody.gravityScale = 3f; 
        }
        else if(rBody.velocity.y < 0) {
            rBody.gravityScale = gravityMultiplier;
        }
        else if(rBody.velocity.y > 0 && !Input.GetButton("Jump")) {

            rBody.gravityScale = gravityMultiplier;
        }
        else if(rBody.velocity.y>0) {
            rBody.gravityScale = 3f;
        }
        else {
            rBody.gravityScale = 1f;
        }
    }

    void Dashing() {
        if(Input.GetButtonDown("Dash") && !dashCD) {

            dashCD = true;
            isInMotion = true;
            rBody.gravityScale = 0;
            rBody.velocity = new Vector2(0, rBody.velocity.y);

            if(facingRight) {
                rBody.AddForce(Vector2.right * dashVelocity, ForceMode2D.Impulse);
            }
            else {
                rBody.AddForce(Vector2.left * dashVelocity, ForceMode2D.Impulse);
            }

            StartCoroutine(Dash(dashTime));
            
            
        }
        
    }

    IEnumerator Dash(float dashTime) {
        yield return new WaitForSeconds(dashTime);
        rBody.gravityScale = 1;
        isInMotion = false;

        yield return new WaitForSeconds(1);
        dashCD = false;
    }

   

    void CollisionChecking() {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, collisionRadius, whatIsGround);

        if(isGrounded) {
            extraJump = true;
            wallRide = false;            
        }
        else {
            wallRide = Physics2D.OverlapCircle(wallChecker.position, collisionRadius, whatIsWall);
            if(wallRide) {
                extraJump = true;
            }
        }
    }
}

