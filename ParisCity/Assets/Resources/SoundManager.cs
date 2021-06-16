using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    public static AudioClip hitsound, jumpsound, pushsound;
    static AudioSource Sour;
   
    void Start()
    {
        hitsound = Resources.Load<AudioClip>("Hit");
        jumpsound = Resources.Load<AudioClip>("Jump");
        pushsound = Resources.Load<AudioClip>("Pushpull");

        Sour = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }


    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Hit":
                Sour.PlayOneShot(hitsound);
                break;
            case "Jump":
                Sour.PlayOneShot(jumpsound);
                break;
            case "Pushpull":
                Sour.PlayOneShot(pushsound);
                break;

        }

        
            

        

    }
}
