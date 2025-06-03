using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogoManager : MonoBehaviour
{
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();    
    }

   public void StartDialogo(Dialogo dialogo)
    {
        Debug.Log("Starting conversation with" + dialogo.name);

        sentences.Clear();

        foreach (string sentence in dialogo.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }

    public void EndDialogue() 
    {
        Debug.Log("Se acabó la conversación");
    }
}
