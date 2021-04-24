using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitties : MonoBehaviour
{
    public Animator animator;
    //Kitty HP
    public int maxHP = 50; //ý made it 50 for the time being incase ý wish to change it to include hurt animation for kitties.
    int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }


    public void GetSmacked(int intdamage)
    {
        currentHP -= intdamage;

        //Kittie smacked animation go here
        animator.SetTrigger("Smacked");


        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Death Animation will go here
        animator.SetBool("Dead", true);
        //destroy the kittie
    }
}
