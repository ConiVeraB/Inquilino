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

    [Header("Chat siguiente (opcional)")]
    public ChatSecuencialDecisiones siguienteChat;

    [Header("Luces a parpadear (opcional)")]
    public LuzParpadeante[] lucesAParpadear;



    private Vector2 posicionVisible = new Vector2(770.72f, 217.71f);
    private Vector2 posicionOculta = new Vector2(1140f, 217.71f);

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

           
                foreach (var luz in lucesAParpadear)
                {
                    if (luz != null)
                    {
                        luz.IniciarParpadeo();
                        Debug.Log($"[{name}] Luz parpadeante activada: {luz.name}");
                    }
                }


                if (siguienteChat != null)
                {
                    Debug.Log($"[{name}] Activando chat directamente tras notificación.");
                    siguienteChat.IniciarDesdeEvento();
                }
            });
        });

        if (sonidoNotificacion != null)
        {
            Debug.Log($"[{name}] Reproduciendo sonido.");
            sonidoNotificacion.Play();
        }
    }

}
