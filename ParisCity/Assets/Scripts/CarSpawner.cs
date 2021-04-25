using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    //objects to spawn
    public GameObject [] spawnObject;
    int spawnObjectIndex;
    GameObject randPrefab, clone;
    
    //spawning time
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;

            spawnObjectIndex = Random.Range(0, spawnObject.Length);
            randPrefab = spawnObject[spawnObjectIndex];
            clone =  GameObject.Instantiate(randPrefab, transform.position, Quaternion.identity) as GameObject;
        }

            
    }
}
