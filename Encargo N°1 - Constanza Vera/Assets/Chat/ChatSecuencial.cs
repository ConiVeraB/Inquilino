using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class ChatSecuencial : MonoBehaviour
{
    [System.Serializable]
    public class Burbuja
    {
        public Image imagenUI;
        public Sprite sprite;
        public AudioClip voz;
        public Vector2 posicionInicial;
        public Vector2 posicionFinal;
    }

    public List<Burbuja> burbujasChat;
    public float delayInicio = 3f;
    public MonoBehaviour controladorCamara;

    private int indiceActual = 0;
    private bool esperandoClick = false;
    private AudioSource chatAudioSource;

    void Start()
    {

        foreach (var b in burbujasChat)
        {
            if (b.imagenUI != null)
                b.imagenUI.gameObject.SetActive(false);
        }


        GameObject audioGO = new GameObject("ChatAudioSource");
        audioGO.transform.SetParent(this.transform);
        chatAudioSource = audioGO.AddComponent<AudioSource>();
        chatAudioSource.playOnAwake = false;
        chatAudioSource.loop = false;
        chatAudioSource.ignoreListenerPause = true;
        chatAudioSource.spatialBlend = 0f;

        StartCoroutine(IniciarChat());
    }

    IEnumerator IniciarChat()
    {
        yield return new WaitForSecondsRealtime(delayInicio);

        //Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (controladorCamara != null)
            controladorCamara.enabled = false;

        yield return MostrarSecuenciaBurbujas();
    }

    IEnumerator MostrarSecuenciaBurbujas()
    {
        while (indiceActual < burbujasChat.Count)
        {
            var burbuja = burbujasChat[indiceActual];

            if (burbuja == null || burbuja.imagenUI == null)
            {
                indiceActual++;
                continue;
            }


            burbuja.imagenUI.rectTransform.anchoredPosition = burbuja.posicionInicial;
            if (burbuja.sprite != null)
                burbuja.imagenUI.sprite = burbuja.sprite;

            burbuja.imagenUI.gameObject.SetActive(true);


            yield return burbuja.imagenUI.rectTransform
                .DOAnchorPos(burbuja.posicionFinal, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .WaitForCompletion();


            if (burbuja.voz != null && chatAudioSource != null)
                chatAudioSource.PlayOneShot(burbuja.voz);

            esperandoClick = true;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            esperandoClick = false;

            indiceActual++;
        }


        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        foreach (var b in burbujasChat)
        {
            if (b.imagenUI != null)
                b.imagenUI.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (controladorCamara != null)
            controladorCamara.enabled = true;
    }
}
