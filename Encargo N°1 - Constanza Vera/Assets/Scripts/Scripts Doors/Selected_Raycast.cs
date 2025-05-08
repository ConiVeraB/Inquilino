using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected_Raycast : MonoBehaviour
{
    public float distancia = 3f;
    void Start()// Start is called before the first frame update
    {
        
    }

    void Update()// Update is called once per frame
    {
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, distancia))
        {
            Debug.DrawRay(transform.position, transform.forward * distancia, Color.red);
            DoorControllerV2 puerta = hit.transform.GetComponentInParent<DoorControllerV2>();
            if (puerta != null && Input.GetKeyDown(KeyCode.E))
            {
                puerta.AlternarPuerta();
            }
        }
    }
    
}
