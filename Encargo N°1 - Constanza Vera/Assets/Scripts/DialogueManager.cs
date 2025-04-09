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
    public PhoneSystem phoneSystem;

    private AudioSource audioSource;
    public AudioClip introAudio;

    void Start()
    {
       component.text = string.Empty;
       gameObject.SetActive(false);
       audioSource = GetComponent<AudioSource>();
        StartDialogueWithAudio();
        // StartDialogue();
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

    public void StartDialogueWithAudio()
    {
        audioSource.PlayOneShot(introAudio);
        StartCoroutine(EsperaAudioEInicioDialogo());
        StartDialogue();
    }
    IEnumerator EsperaAudioEInicioDialogo()
    {
        yield return new WaitForSeconds(introAudio.length);
        StartDialogueWithAudio();
    }
    
    public void StartDialogue()
    {
        gameObject.SetActive(true);
        index = 0;
        component.text = string.Empty;
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

            if (lines[index] == "[Llamada finalizada]")
            {
                phoneSystem.HidePhoneElements(); // Oculta los elementos del teléfono
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
