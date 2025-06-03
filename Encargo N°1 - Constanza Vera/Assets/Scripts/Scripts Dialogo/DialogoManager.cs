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
    }
}
