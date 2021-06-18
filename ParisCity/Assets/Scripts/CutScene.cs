using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class CutScene : MonoBehaviour
{
    public Text textDisplay, continueText;

    public string[] sentences;
    private int index;
    public float typingSpeed, typingSpeed2;
    string continueE = "Press E to continue...";

    void Start()
    {
        StartCoroutine(Type());

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StopAllCoroutines();
            textDisplay.text = "";
            continueText.text = "";
            NextSentence();
        }

        if(sentences[index] == "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }
    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        foreach(char letter in continueE.ToCharArray())
        {
            continueText.text += letter;
            yield return new WaitForSeconds(typingSpeed2);
        }
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
        }
    }
}
