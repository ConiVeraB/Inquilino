using UnityEngine;
using DG.Tweening;
using System.Collections;

public class NotificacionInicioUI : MonoBehaviour
{
    public RectTransform panelNotificacion;
    public float duracionVisible = 4f;
    public float velocidad = 0.5f;

    private Vector2 posicionVisible = new Vector2(770.72f, 217.71f);
    private Vector2 posicionOculta = new Vector2(1140f, 217.71f);

    private void Start()
    {
        if (panelNotificacion == null)
        {
            Debug.LogError("Panel no asignado.");
            return;
        }

       
        panelNotificacion.anchoredPosition = posicionOculta;

        StartCoroutine(MostrarNotificacionConRetraso());
    }

    private IEnumerator MostrarNotificacionConRetraso()
    {
        yield return new WaitForSeconds(3f);
        MostrarNotificacion();
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
    }
}

