using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Kitties : MonoBehaviour
{
    //public Animator animator;
    //Kitty HP
    public int maxHP = 50; //I made it 50 for the time being incase I wish to change it to include hurt animation for kitties.
    int currentHP;

    public Transform Marie;

    public float attackRange, attackDelay;
    private float lastAttack;

    private void Awake()
    {
        currentHP = maxHP;
 
    }

    private void Update()
    {
        //idle animation 

        // check to see if Marie is close enough to attack her
        float distanceToMarie = Vector3.Distance(transform.position, Marie.position);
        if(distanceToMarie < attackRange)
        {
            //check to see if enough time has passed since last attack
            if(Time.time > lastAttack + attackDelay)
            {
                Attack();
                //record last attack
                lastAttack = Time.time;
            }
           
        }
    }

    void Attack()
    {
        //trigger attacking animation 
        
        //Marie takes damgae from cats one hit is 20
        Marie.GetComponent<MarieController>().HP.ReduceHealth(30);
        Marie.GetComponent<MarieController>().animator.SetTrigger("Smacked");

    }
    public void GetSmacked(int intdamage)
    {
        currentHP -= intdamage;

        //Kittie smacked animation go here
      //  animator.SetTrigger("Smacked");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Death Animation will go here
      //  animator.SetBool("Dead", true);
        //destroy the kittie
        Destroy(gameObject);
    }
}
