using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public GameObject[] hearts;
    public int currentHealth = 90;

    void Update()
    {
        if(currentHealth < 30)
        {
            Destroy(hearts[0].gameObject);
        }else if(currentHealth < 60)
        {
            Destroy(hearts[1].gameObject);
        }else if(currentHealth < 90)
        {
            Destroy(hearts[2].gameObject);
        }

        
    }
    public void ReduceHealth(int damage)
    {
        currentHealth -= damage; 
    }
}
