using UnityEngine;
using UnityEngine.UI;

public class BotonDebug : MonoBehaviour
{
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        Image img = GetComponent<Image>();
        if (img != null)
            img.color = Color.red;

        Button btn = GetComponent<Button>();
        if (btn != null)
            Debug.Log("Botón listo: " + gameObject.name);
    }
}
