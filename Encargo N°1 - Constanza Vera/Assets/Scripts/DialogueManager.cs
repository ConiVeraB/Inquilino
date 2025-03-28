using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI component;
    public string[] lines;
    public float textSpeed;

    private int index;


    void Start()
    {
       component.text = string.Empty;
        StartDialogue();
    }

   
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            if (component.text == lines[index])
            {
                NextLine(); 
            }
            else
            {
                StopAllCoroutines();
                component.text = lines[index];  
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            component.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            component.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
