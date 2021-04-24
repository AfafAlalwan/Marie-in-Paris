using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private GameMaster gm;

    private void Start()
    {
        //get reference to the game master to save Marie's last check point
        gm = GameObject.FindGameObjectWithTag("gameMaster").GetComponent<GameMaster>();
    }
    //function gets called as soon as player collides with the checkpoint
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Marie"))
        {
            gm.lastCheckPointPos = transform.position;
        }
    }
}
