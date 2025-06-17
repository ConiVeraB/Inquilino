using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

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

    [Header("Enemigo (opcional)")]
    public EnemyPatrol enemigo;
    public bool iniciarPatrulla = false;
    public bool detenerPatrulla = false;

    [Header("Ruta nueva (Vector3) (opcional)")]
    public Vector3[] nuevaRutaVector;
    public bool sobrescribirRuta = false;

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

        if (enemigo != null && detenerPatrulla)
        {
            enemigo.DetenerPatrulla();
            Debug.Log($"[{name}] Patrulla detenida al inicio");
        }
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

          
                if (enemigo != null)
                {
                    if (sobrescribirRuta && nuevaRutaVector != null && nuevaRutaVector.Length > 0)
                    {
                        
                        enemigo.DetenerPatrulla();

                        List<Transform> puntosTemporales = new List<Transform>();
                        foreach (Vector3 punto in nuevaRutaVector)
                        {
                            GameObject temp = new GameObject("PuntoRutaTemp_" + punto);
                            temp.transform.position = punto;
                            puntosTemporales.Add(temp.transform);
                        }

                        enemigo.puntos = puntosTemporales.ToArray();
                        Debug.Log($"[{name}] Ruta del enemigo sobrescrita con {puntosTemporales.Count} puntos.");
                    }




                    if (iniciarPatrulla)
                    {
                        enemigo.permitirPatrulla = true;
                        Debug.Log($"[{name}] Iniciando patrulla del enemigo.");
                        enemigo.IniciarPatrulla();
                    }

                    if (detenerPatrulla)
                    {
                        Debug.Log($"[{name}] Deteniendo patrulla del enemigo.");
                        enemigo.DetenerPatrulla();
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
