using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarieController : MonoBehaviour
{
    private GameMaster gm;
    SpriteRenderer _renderer;
    Rigidbody2D _rb;
    public float RunningSpeed = 1f, JumpForce = 5f;
    public HealthBar HP;
    public Transform Paws;
    //Animation code
    public Animator animator;
    //Weapon Code
    public Transform BaguettePos;
    public float fBaguetteRange = 0.5f;
    public float fsmackRate = 3f;
    float fnextSmack = 0f;
    

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
        float horizontal = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("Running", true);
            horizontal = -1;
            //Makes animation play when running
            _renderer.flipX = true;


        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
            _renderer.flipX = false;
            //Makes animation play when running
            animator.SetBool("Running", true);

        }

        if (horizontal == 0)
        {
            //Stopping the Animation
            animator.SetBool("Running", false);
        }

        _rb.velocity = new Vector2(horizontal * RunningSpeed, _rb.velocity.y);

        bool grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            //Jump Animation Trigger Code (Trigger so it doesn't loop)
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

    }
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(Paws.position, Vector2.down, 1000, LayerMask.GetMask("Ground"));

        Debug.Log(hit.distance);
        //  return (hit.distance < 0.1f);
        return true;
    }

    void CheckPoint()
    {
        if (HP.currentHealth == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            HP.currentHealth = 100;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Car")
        {
            HP.ReduceHealth(100);
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

    //I'm using this to determine range so I can see the spehere on scene
    private void OnDrawGizmosSelected()
    {
        if (BaguettePos == null)
            return;

        Gizmos.DrawWireSphere(BaguettePos.position, fBaguetteRange);
    }
}
