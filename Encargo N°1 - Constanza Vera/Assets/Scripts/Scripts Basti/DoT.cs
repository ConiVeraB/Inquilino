using DG.Tweening;
using UnityEngine;

public class DoT : MonoBehaviour
{
    public float openAngle, closeAngle;
    bool isOpen;
    public Ease curveAnimation;

    [Header("Sonidos")]
    public AudioSource puerta;
    public AudioClip abrirPuertas;
    public AudioClip cerrarPuertas;

    private bool isMoving = false; // control de animación
    private Tween currentTween; // para asegurar que no se acumulen

    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetOpenDoor();
        }
    }

    public void SetOpenDoor()
    {
        if (isMoving) return; // evita doble activación

        isMoving = true;

        // Detiene cualquier animación previa si aún estaba activa
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        isOpen = !isOpen;

        Vector3 targetRotation = new Vector3(0, isOpen ? openAngle : closeAngle, 0);
        AudioClip clip = isOpen ? abrirPuertas : cerrarPuertas;

        currentTween = transform.DOLocalRotate(targetRotation, 1f, RotateMode.Fast)
            .SetEase(curveAnimation)
            .OnComplete(() => isMoving = false); // desbloqueo

        ReproducirSonido(clip);
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (puerta != null && clip != null)
        {
            puerta.Stop(); // OPCIONAL: corta el sonido actual antes de reproducir el nuevo
            puerta.PlayOneShot(clip);
        }
    }
}
