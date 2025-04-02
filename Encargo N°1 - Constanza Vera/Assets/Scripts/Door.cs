using UnityEngine;

public class Door : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el personaje
        if (other.CompareTag("Player"))
        {
            // Destruye el objeto
            Destroy(gameObject);
        }
    }
}
