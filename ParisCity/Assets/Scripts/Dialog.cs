using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public TextMeshProUGUI continueE;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public Image SpeechBox, blackScreen;
    bool triggering;
    public Transform Marie;
    int count;

    public AudioSource NPCsound; // sound declarer for NPC's

    private void Start()
    {
        StartCoroutine(Type());
        continueE.text = " ";
        SpeechBox.enabled = false;
        blackScreen.enabled = false;
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.E) && triggering && !Marie.GetComponent<MarieController>().equipSpray)
        {
            StopAllCoroutines();
            textDisplay.text = "";
            NextSentence();
            count++;
        }

        if(this.gameObject.tag == "Pierre")
        {
            if(count > 9)
            {
                Marie.GetComponent<MarieController>().isGliding = true;
            }
        }
    }
    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text =  "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit2D);
        if (other.gameObject.tag == "Marie")
        {
            triggering = true;
            continueE.gameObject.SetActive(true);
            continueE.text = "Press E to continue...";
            textDisplay.gameObject.SetActive(true);
            SpeechBox.enabled = true;
            blackScreen.enabled = true;

            NPCsound.Play();

            if (Marie.GetComponent<MarieController>().equipSpray)
            {
                Marie.GetComponent<MarieController>().equipSpray = false;
                Marie.GetComponent<MarieController>().canAttack = true;

            }
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

            if (other.gameObject.tag == "Marie")
            {
                triggering = false;
                continueE.text = " ";
                continueE.gameObject.SetActive(false);
                textDisplay.gameObject.SetActive(false);
                SpeechBox.enabled = false;
                blackScreen.enabled = false;

                
            }
        }
        catch(System.Exception e)
        {
            Debug.Log("meow");
        }
    }

    

}
