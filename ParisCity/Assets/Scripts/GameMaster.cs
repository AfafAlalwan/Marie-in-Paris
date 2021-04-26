using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    private void Awake()
    {

        // if there wasn't an instance set this one to it
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance); //To not let the object destroy itself between scenes and doesn't reset all of it's info
        }
        else
        { // if there was an instance destroy it so there won't be multiple game masters in the same scene
            if (gameObject.CompareTag("Marie"))
            {
                Destroy(gameObject);

            }
        }
    }
}
