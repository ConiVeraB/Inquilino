using UnityEngine;
using UnityEngine.UI;

public class CursorLimiter : MonoBehaviour
{
    public RectTransform areaCelular;
    public Texture2D cursorInvisible;
    public Texture2D cursorNormal;
    public Camera uiCamera; 

    private bool cursorEstaDentro = false;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        bool dentro = RectTransformUtility.RectangleContainsScreenPoint(
            areaCelular,
            mousePos,
            uiCamera != null ? uiCamera : null
        );

        if (dentro && !cursorEstaDentro)
        {
            Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.Auto);
            cursorEstaDentro = true;
        }
        else if (!dentro && cursorEstaDentro)
        {
            Cursor.SetCursor(cursorInvisible, Vector2.zero, CursorMode.Auto);
            cursorEstaDentro = false;
        }
    }
}


