using UnityEngine;

public class DesaparecerAlTocar : MonoBehaviour
{
    void Start()
    {
        
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false); 
        }
    }
}
