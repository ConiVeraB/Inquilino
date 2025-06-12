using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class ChatSecuencialDecisiones : MonoBehaviour
{
    [System.Serializable]
    public class Opcion
    {
        public Button boton;
        public AudioClip audioOpcion;
        public int siguienteIndice; 
    }

    [System.Serializable]
    public class Burbuja
    {
        public Image imagenUI;
        public Sprite sprite;
        public AudioClip voz;
        public Vector2 posicionInicial;
        public Vector2 posicionFinal;
        public bool tieneDecision;
        public GameObject panelDecisiones;
        public List<Opcion> opciones;
        public bool usarSiguientePersonalizado;
        public int siguienteIndiceManual;
        public bool esUltimaBurbuja; 



    }



    [Header("Configuración General")]
    public List<Burbuja> burbujasChat;
    public float delayInicio = 2f;
    public MonoBehaviour controladorCamara;
    public GameObject triggerPrevio;
    public TriggerDeChat siguienteTrigger;

    [Header("Trigger")]
    private bool yaMostrada = false;

    private int indiceActual = 0;
    private bool esperandoClick = false;
    private AudioSource chatAudioSource;




    private void Start()
    {
        foreach (var b in burbujasChat)
        {
            if (b.imagenUI != null)
            {
             
                b.imagenUI.rectTransform.anchoredPosition = b.posicionInicial;
            }

            if (b.panelDecisiones != null)
                b.panelDecisiones.SetActive(false);
        }
        
        if (siguienteTrigger != null)
        {
            siguienteTrigger.ActivarChat(); 
        }


        GameObject audioGO = new GameObject("ChatAudioSource");
        audioGO.transform.SetParent(this.transform);
        chatAudioSource = audioGO.AddComponent<AudioSource>();
        chatAudioSource.playOnAwake = false;
        chatAudioSource.loop = false;
        chatAudioSource.ignoreListenerPause = true;
        chatAudioSource.spatialBlend = 0f;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || yaMostrada)
            return;

        if (triggerPrevio != null)
        {
            var previo = triggerPrevio.GetComponent<ChatSecuencialDecisiones>();
            if (previo != null && !previo.yaMostrada)
                return;
        }

        yaMostrada = true;
        StartCoroutine(IniciarChat());
    }

    public void IniciarDesdeEvento()
    {
        if (yaMostrada) return;
        yaMostrada = true;
        StartCoroutine(IniciarChat());
    }

    IEnumerator IniciarChat()
    {
        yield return new WaitForSecondsRealtime(delayInicio);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (controladorCamara != null)
            controladorCamara.enabled = false;

        yield return MostrarSecuenciaBurbujas();
    }

    IEnumerator MostrarSecuenciaBurbujas()
    {
        while (indiceActual >= 0 && indiceActual < burbujasChat.Count)
        {
            var burbuja = burbujasChat[indiceActual];

            if (burbuja == null || burbuja.imagenUI == null)
            {
                Debug.LogWarning($"Burbuja {indiceActual} inválida.");
                break;
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

         
            if (burbuja.voz != null)
                chatAudioSource.PlayOneShot(burbuja.voz);

           
            if (burbuja.tieneDecision && burbuja.panelDecisiones != null && burbuja.opciones.Count > 0)
            {
                burbuja.panelDecisiones.SetActive(true);

                bool opcionElegida = false;
                int siguiente = -1;

                foreach (var opcion in burbuja.opciones)
                {
                    int destino = opcion.siguienteIndice;
                    opcion.boton.onClick.RemoveAllListeners();
                    opcion.boton.onClick.AddListener(() =>
                    {
                        if (opcion.audioOpcion != null)
                            chatAudioSource.PlayOneShot(opcion.audioOpcion);

                        siguiente = destino;
                        opcionElegida = true;
                        burbuja.panelDecisiones.SetActive(false);
                    });
                }

                yield return new WaitUntil(() => opcionElegida);

                indiceActual = siguiente;
                continue; 
            }

           
            if (burbuja.esUltimaBurbuja)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

               
                foreach (var b in burbujasChat)
                {
                    if (b.imagenUI != null)
                        b.imagenUI.rectTransform.anchoredPosition = b.posicionInicial;

                    if (b.panelDecisiones != null)
                        b.panelDecisiones.SetActive(false);
                }

               
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (controladorCamara != null)
                    controladorCamara.enabled = true;

                yield break; 
            }
            else
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            }


            
            if (burbuja.usarSiguientePersonalizado)
            {
                indiceActual = burbuja.siguienteIndiceManual;
            }
            else
            {
                indiceActual++;
            }
        }


        
        if (indiceActual >= 0 && indiceActual < burbujasChat.Count)
        {
            var burbujaFinal = burbujasChat[indiceActual];

            if (burbujaFinal != null && burbujaFinal.esUltimaBurbuja)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

               
                foreach (var b in burbujasChat)
                {
                    if (b.imagenUI != null)
                        b.imagenUI.rectTransform.anchoredPosition = b.posicionInicial;

                    if (b.panelDecisiones != null)
                        b.panelDecisiones.SetActive(false);
                }

               
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (controladorCamara != null)
                    controladorCamara.enabled = true;

                yield break;
            }
        }


    }


}
