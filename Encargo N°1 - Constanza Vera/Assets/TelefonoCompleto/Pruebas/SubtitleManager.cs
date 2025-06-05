using System.Collections;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;

    [Header("UI de subtítulos")]
    public TextMeshProUGUI subtitleText;
    public CanvasGroup subtitleCanvas;
    public float fadeDuration = 0.25f;

    private Coroutine currentSubtitle;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        subtitleText.text = "";
        subtitleCanvas.alpha = 0;
    }

    public void ShowSubtitle(string text, float duration)
    {
        if (currentSubtitle != null)
            StopCoroutine(currentSubtitle);

        currentSubtitle = StartCoroutine(SubtitleRoutine(text, duration));
    }

    IEnumerator SubtitleRoutine(string text, float duration)
    {
        subtitleText.text = text;

        
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        subtitleCanvas.alpha = 1;
        yield return new WaitForSeconds(duration);

      
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            subtitleCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        subtitleText.text = "";
    }
}
