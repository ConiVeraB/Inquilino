using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imagenBoton;
    public Color colorNormal = Color.white;
    public Color colorHover = new Color(0.9f, 0.9f, 1f); // azul claro

    void Start()
    {
        if (imagenBoton == null)
            imagenBoton = GetComponent<Image>();

        imagenBoton.color = colorNormal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        imagenBoton.color = colorHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imagenBoton.color = colorNormal;
    }
}
