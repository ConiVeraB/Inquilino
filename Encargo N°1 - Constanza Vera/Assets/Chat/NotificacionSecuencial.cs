using UnityEngine;
using DG.Tweening;

public class NotificacionSecuencial : MonoBehaviour
{
    [Header("UI")]
    public RectTransform panelNotificacion;
    public AudioSource sonidoNotificacion;

    [Header("Animación")]
    public float duracionVisible = 4f;
    public float velocidad = 0.5f;

    [Header("Secuencia")]
    public GameObject triggerPrevio; 

    [SerializeField] Vector2 posicionVisible = new Vector2(770.72f, 217.71f);
    [SerializeField] Vector2 posicionOculta = new Vector2(1140f, 217.71f);

    [HideInInspector] public bool yaMostrada = false;

    private void Start()
    {
        if (panelNotificacion == null)
        {
            Debug.LogError($"[{name}] Panel de notificación no asignado.");
            return;
        }

        panelNotificacion.anchoredPosition = posicionOculta;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[{name}] OnTriggerEnter detectado con: {other.name}");

        if (!other.CompareTag("Player"))
        {
            Debug.Log($"[{name}] El objeto que entró no tiene el tag 'Player'.");
            return;
        }

        if (yaMostrada)
        {
            Debug.Log($"[{name}] Ya fue mostrada, no se repite.");
            return;
        }

        if (triggerPrevio != null)
        {
            NotificacionSecuencial scriptPrevio = triggerPrevio.GetComponent<NotificacionSecuencial>();

            if (scriptPrevio == null)
            {
                Debug.LogWarning($"[{name}] El objeto previo no tiene el script NotificacionSecuencial.");
                return;
            }

            if (!scriptPrevio.yaMostrada)
            {
                Debug.Log($"[{name}] El trigger anterior aún no fue activado.");
                return;
            }
        }

        Debug.Log($"[{name}] Mostrando notificación.");
        MostrarNotificacion();
        yaMostrada = true;
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
        {
            Debug.Log($"[{name}] Reproduciendo sonido.");
            sonidoNotificacion.Play();
        }
    }
}
