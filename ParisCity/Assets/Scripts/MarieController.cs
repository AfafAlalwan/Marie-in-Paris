using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarieController : MonoBehaviour
{
    private GameMaster gm;
    SpriteRenderer _renderer;
    Rigidbody2D _rb;
    
    //  public HealthBar HP;
    public HealthSystem HP;
    public Transform Paws;
    public LayerMask GroundLayer;
    //Animation code
    public Animator animator;
    //Weapon Code
    public Transform BaguettePos;
    public float fBaguetteRange = 0.5f;
    public float fsmackRate = 3f;
    float fnextSmack = 0f;

    //for pushing
    public float PushDistance = 1f;
    public LayerMask PushMask;
    GameObject box;

    //for jumping
    public float jumpForce;

    // for running 
    private float moveInput;
    private float runSpeed = 5f;
    public float maxSpeed = 10f;


    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        //starting position 
        gm = GameObject.FindGameObjectWithTag("gameMaster").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }


    void Update()
    {
        MoveMarie();
        CheckPoint();

    }

  
    void MoveMarie()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(moveInput * runSpeed, _rb.velocity.y);

        if(moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetTrigger("Run Transition");
            animator.SetBool("Running", true);
            if (runSpeed < maxSpeed)
            {
                runSpeed += Time.deltaTime;
            }
        }
        else if(moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetTrigger("Run Transition");
            animator.SetBool("Running", true);
            if (runSpeed < maxSpeed)
            {
                runSpeed += Time.deltaTime;
            }
        }

        if (moveInput == 0)
        {
            runSpeed = 5f;
            animator.SetBool("Running", false);
        }


        //Jumping
        bool grounded = Physics2D.OverlapCircle(Paws.position, 0.2f, GroundLayer);


        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            _rb.AddForce(Vector2.up * jumpForce * _rb.mass, ForceMode2D.Impulse);
        
            animator.SetTrigger("Jump");
        }


        //If you press Space button it should attack with this code
        if (Time.time >= fnextSmack)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Baguattack();
                //Attack Animation Trigger Code (Trigger so it doesn't loop)
                animator.SetTrigger("Baguattack");
                fnextSmack = Time.time + 1f / fsmackRate;
            }
        }

        //For pushiing and pulling press P 
        //hit from right
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right* transform.localScale.x, PushDistance, PushMask); //I think cuz of this

        //hit from left
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left * transform.localScale.x, PushDistance, PushMask);

        RaycastHit2D hit = hit1;

        if(moveInput > 0)
        {
            hit = hit1;
        }else if(moveInput < 0)
        {
            hit = hit2;
        }

        try
        {if (hit.collider != null && hit.collider.gameObject.tag == "Pushable" && Input.GetKeyDown(KeyCode.P))
        {
            box = hit.collider.gameObject;
            animator.SetBool("Push", true);
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<BoxPushnPull>().beingPushed = true;
            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.GetComponent<BoxPushnPull>().beingPushed = false;
            animator.SetBool("Push", false);
        }

        }
        catch(System.NullReferenceException e)
        {
            Debug.Log("meow");
        }
    }
  

    void CheckPoint()
    {
        if (HP.currentHealth == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            HP.currentHealth = 90; //was 100
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Car")
        {
            HP.ReduceHealth(90); // was 100
            Destroy(other.gameObject);
        }
    }

    //Player Attacking Code
    void Baguattack()
    {
        //Attack Range
        // specific code for checking what to collide. We can use layers so attack only collides with enemy layers.
        // Basicaly Marie chooses what to smack
        Collider2D[] smack = Physics2D.OverlapCircleAll(BaguettePos.position, fBaguetteRange, LayerMask.GetMask("Enemy")); 

        //Damage
        foreach (Collider2D enemy in smack) //it will smack each person that is count as enemy in the reach parameter we made in attack range section
        {
            enemy.GetComponent<Kitties>().GetSmacked(25); //change getsmacked to change dmg number. Kitties are enemy cats.
        }
    }

    //a shpere for attacking range
    private void OnDrawGizmosSelected()
    {
        if (BaguettePos == null)
            return;

        Gizmos.DrawWireSphere(BaguettePos.position, fBaguetteRange);
    }

    //Drawing lines for pullnpush
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,(Vector2)transform.position + Vector2.right * transform.localScale.x * PushDistance);
    }

    
}
