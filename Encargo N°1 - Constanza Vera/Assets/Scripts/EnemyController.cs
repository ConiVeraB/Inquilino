using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void Aparecer()
    {
        gameObject.SetActive(true);
        Debug.Log("El Sujeto ha aparecido.");
    }

    public void Desaparecer()
    {
        gameObject.SetActive(false);
        Debug.Log("El Sujeto ha desaparecido.");
    }
}

//public class EventoPuerta : MonoBehaviour
//{
//    public EnemyController enemigo;

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            enemigo.Aparecer(); // o enemigo.Desaparecer();
//        }
//    }
//}          Este se agrega a cualquier script de evento (Trigger, puertas, etc y se configura desde allí)

