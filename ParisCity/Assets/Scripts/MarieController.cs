using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarieController : MonoBehaviour
{
    private GameMaster gm;
    Rigidbody2D _rb;
    public HealthSystem HP;
    public Transform Paws;

    //Animation code
    public Animator animator;

    //For attack
    public Transform BaguettePos;
    public float fBaguetteRange = 0.5f;
    public float fsmackRate = 3f;
    float fnextSmack = 0f;
    public bool canAttack = false;
    public bool equipSpray = false;

    //For pushing
    public float PushDistance = 1f;
    public LayerMask PushMask;
    GameObject box;
    RaycastHit2D hit;

    //For jumping
    public LayerMask GroundLayer;
    public float jumpForce;
    bool isGrounded;

    //For movement 
    private float moveInputX, moveInputY;
    private float runSpeed = 5f;
    public float maxSpeed = 10f;
    public float AirSpeed = 30f;

    //For wall jump 
    public LayerMask wallLayer;
    public Transform wallCheck;
    public Vector2 wallCheckSize;
    bool isWallSliding;
    bool isTouchingWall;
    public float wallSlidingSpeed;
    public float wallJumpForce = 18f;
    public float wallJumpDirection = -1f;
    public Vector2 wallJumpAngle;

    //For gliding
    public bool isGliding = false;
    public float fallSpeed;
    public Transform Pierre;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        //starting position 
        gm = GameObject.FindGameObjectWithTag("gameMaster").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;

        wallJumpAngle.Normalize();
    }


    void Update()
    {
        MoveMarie();
        CheckPoint();
    }

    void CheckPoint()
    {
        if (HP.currentHealth == 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            HP.currentHealth = 90; 
        }
    }

    void MoveMarie()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");

        if (isGrounded)
        {
            _rb.velocity = new Vector2(moveInputX * runSpeed, _rb.velocity.y);
        }
        else if (!isGrounded && !isWallSliding && moveInputX != 0)
        {
            _rb.AddForce(new Vector2(moveInputX * AirSpeed, 0));
            if (Mathf.Abs(_rb.velocity.x) > runSpeed)
            {
                _rb.velocity = new Vector2(moveInputX * runSpeed, _rb.velocity.y);

            }
        }


        if (moveInputX > 0)
        {
            if (!Input.GetKey(KeyCode.P))
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                wallJumpAngle *= -1f;
            }

            animator.SetBool("Running", true);
            if (runSpeed < maxSpeed)
            {
                runSpeed += Time.deltaTime;
            }
        }
        else if (moveInputX < 0)
        {
            if (!Input.GetKey(KeyCode.P))
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                wallJumpAngle *= -1f;
            }


            animator.SetBool("Running", true);
            if (runSpeed < maxSpeed)
            {
                runSpeed += Time.deltaTime;
            }
        }

        if (moveInputX == 0)
        {
            runSpeed = 5f;
            animator.SetBool("Running", false);
        }


        //Jumping
        isGrounded = Physics2D.OverlapCircle(Paws.position, 0.2f, GroundLayer);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            _rb.AddForce(Vector2.up * jumpForce * _rb.mass, ForceMode2D.Impulse);

            animator.SetTrigger("Jump");

            SoundManager.PlaySound("Jump"); // JUMP SOUND ******
        }

        //For gliding
        if(isGliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, moveInputY * fallSpeed);
        }

        //Press Space to attack
        if (Time.time >= fnextSmack)
        {
            if (Input.GetKey(KeyCode.Space) && canAttack)
            {
                Baguattack();
                animator.SetTrigger("Baguattack");

                SoundManager.PlaySound("Hit"); // HIT SOUND ***

                fnextSmack = Time.time + 1f / fsmackRate;
            }
        }

        //For pushiing and pulling press P 
        //hit from right
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, PushDistance, PushMask);

        //hit from left
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, PushDistance, PushMask);

        hit = hit1;

        if (moveInputX > 0)
        {
            hit = hit1;
        }
        else if (moveInputX < 0)
        {
            hit = hit2;
        }

        try
        {
            if (hit.collider != null && hit.collider.gameObject.tag == "Pushable" && Input.GetKeyDown(KeyCode.P))
            {
                box = hit.collider.gameObject;

                box.GetComponent<FixedJoint2D>().enabled = true;
                box.GetComponent<BoxPushnPull>().beingPushed = true;
                box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();

                //need to find a way to detect if marie moving right or left for pulling -- tried someways and it didn't work 
                animator.SetBool("Push", true);

                SoundManager.PlaySound("Pushpull"); // PUSH SOUND***

                //  animator.SetBool("Pull", true);



            }
            else if (Input.GetKeyUp(KeyCode.P))
            {
                box.GetComponent<FixedJoint2D>().enabled = false;
                box.GetComponent<BoxPushnPull>().beingPushed = false;
                animator.SetBool("Push", false);
                //  animator.SetBool("Pull", false);

            }

        }
        catch (System.NullReferenceException e)
        {
            Debug.Log("meow");
        }

        //Wall jump code
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer);

        if (isTouchingWall && !isGrounded && _rb.velocity.y < 0)
        {
            isWallSliding = true;
            Debug.Log("Touch");
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, wallSlidingSpeed);
        }

        if ((isWallSliding || isTouchingWall) && Input.GetKeyDown(KeyCode.W))
        {
            _rb.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, 
                wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
        }

        //For gliding
        if(isGliding)
        {
            Destroy(Pierre.gameObject);
            animator.SetBool("Glide", true);

            if (_rb.velocity.y < 0f && Mathf.Abs(_rb.velocity.y) > fallSpeed)
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Sign(_rb.velocity.y) * fallSpeed);
        }
        else
        {
            animator.SetBool("Glide", false) ;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Car")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            HP.ReduceHealth(90); 
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "DeathZone")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            HP.ReduceHealth(90);
        }

        if(other.gameObject.tag == "Spray")
        {
            Destroy(other.gameObject);
            equipSpray = true;
        }

        if(other.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

    }

    //Player Attacking Code
    void Baguattack()
    {
        //Attack Range
        Collider2D[] smack = Physics2D.OverlapCircleAll(BaguettePos.position,
            fBaguetteRange, LayerMask.GetMask("Enemy"));
        //Damage
        foreach (Collider2D enemy in smack) 
        {
            enemy.GetComponent<Kitties>().GetSmacked(25); 
        }
    }

    //A shpere for attacking range
    private void OnDrawGizmosSelected()
    {
        if (BaguettePos == null)
            return;

        Gizmos.DrawWireSphere(BaguettePos.position, fBaguetteRange);
    }


    private void OnDrawGizmos()
    {
        //Drawing lines for pullnpush
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * PushDistance);

        //For wall check
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }


}
