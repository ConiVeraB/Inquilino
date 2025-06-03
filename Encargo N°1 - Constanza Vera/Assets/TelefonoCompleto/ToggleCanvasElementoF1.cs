using UnityEngine;
using DG.Tweening;

public class ToggleCanvasElementoF1 : MonoBehaviour
{
    public RectTransform elementoUI;
    public float velocidad = 0.4f;

    private Vector2 posicionVisible = new Vector2(0f, 0f);
    private Vector2 posicionEscondida = new Vector2(0f, 996f);
    private bool animando = false;
    private bool visible = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            if (!visible && !animando)
            {
                MostrarElemento();
            }
        }
        else
        {
            if (visible && !animando)
            {
                OcultarElemento();
            }
        }
    }

    void MostrarElemento()
    {
        if (elementoUI == null) return;

        animando = true;
        visible = true;

        elementoUI.gameObject.SetActive(true);
        elementoUI.DOKill();
        elementoUI.anchoredPosition = posicionEscondida;
        elementoUI.DOAnchorPos(posicionVisible, velocidad).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            animando = false;
        });
    }

    void OcultarElemento()
    {
        if (elementoUI == null) return;

        animando = true;
        elementoUI.DOKill();
        elementoUI.DOAnchorPos(posicionEscondida, velocidad).SetEase(Ease.InCubic).OnComplete(() =>
        {
            visible = false;
            animando = false;
        });
    }
}





