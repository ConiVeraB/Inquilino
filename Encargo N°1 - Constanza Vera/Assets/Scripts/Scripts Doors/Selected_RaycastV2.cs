using UnityEngine;

public class Selected_RaycastV2 : MonoBehaviour
{
    public float distancia = 3.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distancia))
        {
            //Debug.Log("Impactado: " + hit.transform.name);//Diagnóstico: mostrar qué objeto se impacta
            Debug.DrawRay(transform.position, transform.forward * distancia, Color.red);
            
            DoTweenControlAnimV1 puertaV1 = hit.transform.GetComponentInParent<DoTweenControlAnimV1>();// Intentar obtener el script DoTweenControlAnimV1

            //Debug.Log("¡Encontrado DoTweenControlAnim en: " + puerta.name + "!");
            DoTweenControlAnimV2 puertaV2 = hit.transform.GetComponentInParent<DoTweenControlAnimV2>();// Intentar obtener el script DoTweenControlAnimV2

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (puertaV1 != null)
                {
                    puertaV1.SetOpenDoor(); // Método correspondiente de V1
                }
                else if (puertaV2 != null)
                {
                    puertaV2.SetOpenDoor(); // Método correspondiente de V2
                }
            }

        }
    }
}
