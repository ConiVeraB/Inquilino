using UnityEngine;
public class TestPanel : MonoBehaviour
{
    void Start()
    {
        Debug.Log("TestPanel activo");
        gameObject.SetActive(true);

        var cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.color = Color.white;
        }
    }
}

