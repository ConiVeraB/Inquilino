using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhoneSystem : MonoBehaviour
{
    public GameObject Phone;
    public float Delay = 2f;
    public Sprite Sprite;
    public Button Accept;
    public Button Decline;
    public GameObject phoneBackground;
    public DialogueManager dialogueManager;

    void Start()
    {
        Phone.SetActive(true);
        Accept.onClick.AddListener(AcceptCall);
        Decline.onClick.AddListener(DeclineCall);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Phone != null)
            {
                if (!Phone.activeSelf)
                {
                    Phone.SetActive(true); // Muestra el teléfono inmediatamente
                }
                else
                {
                    StartCoroutine(HidePhoneAfterDelay()); // Espera 2 segundos antes de ocultarlo
                }
            }
        }


    }

    IEnumerator HidePhoneAfterDelay()
    {
        yield return new WaitForSeconds(Delay); // Espera 2 segundos
        Phone.SetActive(false); // Oculta el teléfono
    }

    void AcceptCall()
    {
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue(); // Inicia el diálogo
        }
        Phone.SetActive(true); 
    }

    void DeclineCall()
    {
        Phone.SetActive(false); // Solo cierra el teléfono sin iniciar diálogo
    }

    public void HidePhoneElements()
    {
        phoneBackground.SetActive(false);
        Accept.gameObject.SetActive(false);
        Decline.gameObject.SetActive(false);
        Phone.SetActive(false);
    }

}
