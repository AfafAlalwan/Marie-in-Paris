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
            horizontal = -1;
            _renderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
            _renderer.flipX = false;
        }

        _rb.velocity = new Vector2(horizontal * RunningSpeed, _rb.velocity.y);

        bool grounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            _rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }

    }
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(Paws.position, Vector2.down, 1000, LayerMask.GetMask("Ground"));
        return (hit.distance < 0.1f);
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
        if(other.gameObject.tag == "Car")
        {
            HP.ReduceHealth(100);
            Destroy(other.gameObject);
        }
    }
}
