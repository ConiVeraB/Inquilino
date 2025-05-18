using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class DoTweenControlAnimV1 : MonoBehaviour
{
    public float openAngle, closeAngle;
    bool isOpen;
    bool isAnimating = false;
    public Ease curveAnimation;

    [Header("Sonidos")]
    public AudioSource puerta;
    public AudioClip abrirPuerta;
    public AudioClip cerrarPuerta;
    
    void Start()// Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        
    }

    private void Update()// Update is called once per frame
    {
        
    }
    public void SetOpenDoor()
    {

        if (isAnimating) return; // ← NUEVO: evitar múltiples llamadas durante la animación
        isAnimating = true;      // ← NUEVO: bloquear interacción

        isOpen = !isOpen;
        switch (isOpen)
        {
            case true:
                //ABRIR
                transform.DOLocalRotate(new Vector3(0, openAngle, 0), 1f, RotateMode.FastBeyond360).SetEase(curveAnimation).OnComplete(() => isAnimating = false);//positivo
                ReproducirSonido(abrirPuerta);

                break;

            case false:
                //CERRAR
                transform.DOLocalRotate(new Vector3(0, closeAngle, 0), 1f, RotateMode.FastBeyond360).SetEase(curveAnimation).OnComplete(() => isAnimating = false); // ← NUEVO: desbloquear al terminar//.fast es negativo
                ReproducirSonido(cerrarPuerta);
                break;
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (puerta != null && clip != null)
        {
            puerta.PlayOneShot(clip);
        }
    }
}
