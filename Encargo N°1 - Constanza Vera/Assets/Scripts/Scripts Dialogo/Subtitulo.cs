using UnityEngine;
using TMPro;
using System.Collections;

public class Subtitulo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtituloTexto;

    public static Subtitulo instance;
    private void Awake()
    {
        instance = this;
        ClearSubtitle();
    }

    public void SetSubtitle(string subtitle, float delay)
    {
        subtituloTexto.text = subtitle;
        StartCoroutine(ClearAfterSeconds(delay));
    }

    public void ClearSubtitle()
    {
        subtituloTexto.text = "";
    }

    private IEnumerator ClearAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay); 
        ClearSubtitle();
    }
}
