using UnityEngine;
using DG.Tweening;

public class NotificacionCafe : MonoBehaviour
{
    [Header("UI")]
    public RectTransform panelNotificacion;
    public AudioSource sonidoNotificacion;

    [Header("Animación")]
    public float duracionVisible = 4f;
    public float velocidad = 0.5f;

    [SerializeField] Vector2 posicionVisible = new Vector2(766f, 342f);
    [SerializeField] Vector2 posicionOculta = new Vector2(1141f, 342f);

    private bool yaMostrada = false;

    private void Start()
    {
        if (panelNotificacion == null)
        {
            Debug.LogError("Panel de notificación no asignado.");
            return;
        }

        panelNotificacion.anchoredPosition = posicionOculta;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaMostrada) return;

        if (other.CompareTag("Player"))
        {
            MostrarNotificacion();
            yaMostrada = true;
        }
    }

    public void MostrarNotificacion()
    {
        if (panelNotificacion == null) return;

        panelNotificacion.DOKill();
        panelNotificacion.anchoredPosition = posicionOculta;

        panelNotificacion.DOAnchorPos(posicionVisible, velocidad).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            DOVirtual.DelayedCall(duracionVisible, () =>
            {
                panelNotificacion.DOAnchorPos(posicionOculta, velocidad).SetEase(Ease.InCubic);
            });
        });

        if (sonidoNotificacion != null)
            sonidoNotificacion.Play();
    }
}

