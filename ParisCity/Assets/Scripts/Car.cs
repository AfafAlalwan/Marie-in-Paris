using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody2D _rb;
    private Vector2 screenBounds;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(-1 * speed, _rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //removing cars from scene at the end of the road
        if (other.gameObject.tag == "End")
        {
            Destroy(this.gameObject);
        }

       
    }
    

}
