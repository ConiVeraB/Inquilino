using System.Collections;
using UnityEngine;

public class PhoneSystem : MonoBehaviour
{
    public GameObject Phone;
    public float Delay = 2f;
    void Start()
    {
        Phone.SetActive(false);
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
}
