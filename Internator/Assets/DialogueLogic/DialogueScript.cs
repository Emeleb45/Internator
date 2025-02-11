using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public float typingSpeed;
    private int index;
    void Start()
    {
        gameObject.SetActive(true);
        textDisplay.text = "";
        startDialogue();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (textDisplay.text == sentences[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                textDisplay.text = sentences[index];
            }
        }
    }

    void startDialogue()
    {
        index = 0;
        StartCoroutine(Type());
    }
    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    void nextLine()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            gameObject.SetActive(false);
            textDisplay.text = "";
        }
    }
}