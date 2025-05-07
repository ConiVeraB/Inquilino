using UnityEngine;

public class DoorControllerV2 : MonoBehaviour
{
    private Animator anim;
    private bool puertaAbierta = false;
    void Start()// Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("Animator no encontrado en DoorPivot");
    }

    
    void Update()// Update is called once per frame
    {
        
    }
    public void AlternarPuerta()
    {
        if (anim == null) return;

        if (puertaAbierta)
        {
            anim.SetTrigger("cerrar");
            puertaAbierta = false;
        }
        else
        {
            anim.SetTrigger("abrir");
            puertaAbierta = true;
        }
    }
}
