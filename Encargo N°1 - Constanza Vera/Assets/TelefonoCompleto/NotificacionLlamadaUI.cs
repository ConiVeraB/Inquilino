using UnityEngine;
using DG.Tweening;

public class NotificacionLlamadaUI : MonoBehaviour
{
    public RectTransform panelNotificacion;
    public float duracionVisible = 4f;
    public float velocidad = 0.5f;

    private Vector2 posicionVisible = new Vector2(770.72f, 217.71f);
    private Vector2 posicionOculta = new Vector2(1140f, 217.71f);

    private void Start()
    {
        if (panelNotificacion == null) return;

      
        panelNotificacion.anchoredPosition = posicionOculta;
        panelNotificacion.gameObject.SetActive(false);
    }

    public void MostrarNotificacion()
    {
        if (panelNotificacion == null) return;

        panelNotificacion.gameObject.SetActive(true);
        panelNotificacion.DOKill();
        panelNotificacion.anchoredPosition = posicionOculta;

      
        panelNotificacion.DOAnchorPos(posicionVisible, velocidad).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            DOVirtual.DelayedCall(duracionVisible, () =>
            {
                panelNotificacion.DOAnchorPos(posicionOculta, velocidad).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    panelNotificacion.gameObject.SetActive(false);
                });
            });
        });
    }
}




