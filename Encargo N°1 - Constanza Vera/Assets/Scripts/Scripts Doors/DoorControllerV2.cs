using UnityEngine;

public class DoorControllerV2 : MonoBehaviour
{
    private Animator anim;
    private bool puertaAbierta = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

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
            ReproducirSonido(sonidoCerrar);
            anim.SetTrigger("cerrar");
            puertaAbierta = false;
        }
        else
        {
            anim.SetTrigger("abrir");
            ReproducirSonido(sonidoAbrir);
            puertaAbierta = true;
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
