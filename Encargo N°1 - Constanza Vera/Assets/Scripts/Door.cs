using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioSource doorSource;
    public Animator animator;

    public void OpenDoor()
    {
        animator.SetTrigger("Door");
        doorSource.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el personaje
        if (other.CompareTag("Player"))
        {
            
            OpenDoor();
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            animator.ResetTrigger("Door");
        }
    }*/
}
