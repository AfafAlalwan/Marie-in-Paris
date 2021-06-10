using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRandomizer : MonoBehaviour
{
    public AudioSource Music1;
    public AudioSource Music2;
    public AudioSource Music3;

    public int MusicSelector; //Random selector

    public int History; //for avoiding replays

    void Start()
    {
        MusicSelector = Random.Range(0, 3);
        
        if(MusicSelector== 0)
        {
            Music1.Play();
            History = 1;
        }
        else if (MusicSelector == 1)
        {
            Music2.Play();
            History = 2;
        }
        else if (MusicSelector == 2)
        {
            Music3.Play();
            History = 3;
        }

    }
     void Update()
    {
        if(Music1.isPlaying == false && Music2.isPlaying == false && Music3.isPlaying == false)
        {
            MusicSelector = Random.Range(0, 3);

            if (MusicSelector == 0 && History!=1)
            {
                Music1.Play();
                History = 1;
            }
            else if (MusicSelector == 1 && History!= 2)
            {
                Music2.Play();
                History = 2;
            }
            else if (MusicSelector == 2 && History!=3)
            {
                Music3.Play();
                History = 3;
            }
        }
    }

}
