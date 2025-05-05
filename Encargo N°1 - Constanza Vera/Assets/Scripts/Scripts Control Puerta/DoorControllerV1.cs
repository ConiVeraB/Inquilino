using UnityEngine;

public class DoorControllerV1 : MonoBehaviour
{
    [SerializeField] GameObject door; // Asigna aquí el DoorPivot, que debe tener el Animator

    private Animator anim;

    void Start()
    {
        anim = door.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("El Animator no está asignado en el GameObject de la puerta");
        }
    }

    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("abrir");
            print ("Se abrió la puerta.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && anim != null)
        {
            anim.SetTrigger("cerrar");
            print("Se cerró la puerta.");
        }
    }
}
