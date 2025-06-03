using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

[System.Serializable]
public class DatosLlamada
{
    public string nombre;
    public Sprite imagenContacto;
    public float tiempoInicio; 
    public GameObject panelLlamada; 
    public GameObject panelEnLlamada; 
    public float duracionMaxima = 10f;
    public AudioObject audioAndSubtitles;
    //public AudioClip audioConversacion; 
}

public class LlamadaManager : MonoBehaviour
{
    [Header("Configuración de llamadas")]
    public List<DatosLlamada> llamadas;

    [Header("Elementos UI")]
    public Image imagenContactoUI;
    public TextMeshProUGUI textoDuracionLlamada;
    public Button botonContestar;
    public Button botonCortar;
    public Button botonCortarEnLlamada;

    [Header("Sonido")]
    public AudioSource audioLlamada;      
    public AudioClip clipLlamada;

    [Header("Audio conversación")]
    public AudioSource audioConversacion; 

    [Header("Cortar/Contestar")]
    public AudioSource audioSource;
    public AudioClip Touch;

    [Header("Vibración")]
    public RectTransform imagenAVibrar;
    public float intensidadVibracion = 10f;
    public float frecuenciaVibracion = 25f;

    [Header("Notificación de llamada")]
    public NotificacionLlamadaUI notificacionLlamadaUI;

    [Header("Notificación post-llamada")]
    public NotificacionPostLlamadaUI notificacionPostLlamadaUI;

    [SerializeField] private AudioClip audioNegacion;
    [SerializeField] private AudioSource audioUI;



    private float tiempoJuego;
    private int indiceActual = 0;
    private bool llamadaEnCurso = false;
    private Coroutine llamadaActiva;
    private bool vibrando = false;
    private Vector3 posicionOriginal;
    private bool audioNegacionReproduciendose = false;

    void Update()
    {
        tiempoJuego += Time.deltaTime;

        if (indiceActual < llamadas.Count && !llamadaEnCurso)
        {
            var llamada = llamadas[indiceActual];
            if (tiempoJuego >= llamada.tiempoInicio)
            {
                MostrarLlamadaEntrante(llamada);
                llamadaEnCurso = true;
            }
        }

        if (vibrando && imagenAVibrar != null)
        {
            float offsetX = Mathf.Sin(Time.time * frecuenciaVibracion) * intensidadVibracion;
            float offsetY = Mathf.Cos(Time.time * frecuenciaVibracion) * intensidadVibracion;
            imagenAVibrar.anchoredPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0);
        }
    }

    void MostrarLlamadaEntrante(DatosLlamada llamada)
    {
        llamada.panelLlamada?.SetActive(true);

        if (imagenContactoUI != null && llamada.imagenContacto != null)
            imagenContactoUI.sprite = llamada.imagenContacto;

        if (audioLlamada != null && clipLlamada != null)
        {
            audioLlamada.clip = clipLlamada;
            audioLlamada.loop = true;
            audioLlamada.Play();
        }

        if (imagenAVibrar != null)
        {
            posicionOriginal = imagenAVibrar.anchoredPosition;
            vibrando = true;
        }

        botonContestar.onClick.RemoveAllListeners();
        botonContestar.onClick.AddListener(() => ContestarLlamada(llamada));

        botonCortar.onClick.RemoveAllListeners();
        botonCortar.interactable = true;
        botonCortar.gameObject.SetActive(true);

        if (indiceActual == 0)
        {
            
            botonCortar.onClick.AddListener(ReproducirAudioNegacion);
        }
        else
        {
          
            botonCortar.onClick.AddListener(() => CortarLlamada(llamada));
        }


        //botonCortarEnLlamada.onClick.RemoveAllListeners();
        //botonCortarEnLlamada.onClick.AddListener(() => CortarLlamada(llamada));

        if (notificacionLlamadaUI != null)
            Invoke(nameof(MostrarNotificacionConRetraso), 2f);
    }


    void ReproducirAudioNegacion()
    {
        if (audioUI == null || audioNegacion == null || audioNegacionReproduciendose)
            return;

        audioUI.PlayOneShot(audioNegacion);
        audioNegacionReproduciendose = true;

        StartCoroutine(ResetearBloqueoAudio(audioNegacion.length));
    }

    IEnumerator ResetearBloqueoAudio(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        audioNegacionReproduciendose = false;
    }


    void MostrarNotificacionConRetraso()
    {
        if (notificacionLlamadaUI != null)
            notificacionLlamadaUI.MostrarNotificacion();
    }



    void ContestarLlamada(DatosLlamada llamada)
    {
        llamada.panelLlamada?.SetActive(false);
        llamada.panelEnLlamada?.SetActive(true);
        Vocals.instance.Say(llamada.audioAndSubtitles);

        if (audioLlamada != null && audioLlamada.isPlaying)
            audioLlamada.Stop();

        if (vibrando && imagenAVibrar != null)
        {
            imagenAVibrar.anchoredPosition = posicionOriginal;
            vibrando = false;
        }

        if (textoDuracionLlamada != null)
            textoDuracionLlamada.text = "00:00";

 
        botonCortarEnLlamada.interactable = false;
        botonCortarEnLlamada.gameObject.SetActive(false);

        llamadaActiva = StartCoroutine(ContarDuracionLlamada(llamada));

      
        if (audioConversacion != null && llamada.audioAndSubtitles.clip != null)
        {
            audioConversacion.clip = llamada.audioAndSubtitles.clip;
            audioConversacion.Play();
            StartCoroutine(EsperarFinConversacion(llamada.audioAndSubtitles.clip.length, llamada));
        }
        PlayButtonSound();
    }

    IEnumerator ContarDuracionLlamada(DatosLlamada llamada)
    {
        float duracion = 0f;

        while (duracion < llamada.duracionMaxima)
        {
            duracion += Time.deltaTime;

            if (textoDuracionLlamada != null)
            {
                int minutos = Mathf.FloorToInt(duracion / 60f);
                int segundos = Mathf.FloorToInt(duracion % 60f);
                textoDuracionLlamada.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            }

            yield return null;
        }
        PlayButtonSound();
    }

    IEnumerator EsperarFinConversacion(float duracion, DatosLlamada llamada)
    {
        yield return new WaitForSeconds(duracion);


        FinalizarLlamada(llamada);
    }

    void CortarLlamada(DatosLlamada llamada)
    {
        if (audioLlamada != null && audioLlamada.isPlaying)
            audioLlamada.Stop();

        if (audioConversacion != null && audioConversacion.isPlaying)
            audioConversacion.Stop();

        if (llamadaActiva != null)
            StopCoroutine(llamadaActiva);
        PlayButtonSound();
        FinalizarLlamada(llamada);
    }

    void FinalizarLlamada(DatosLlamada llamada)
    {
        llamada.panelLlamada?.SetActive(false);
        llamada.panelEnLlamada?.SetActive(false);

        indiceActual++;
        llamadaEnCurso = false;

        if (notificacionPostLlamadaUI != null)
            StartCoroutine(MostrarPostLlamadaConRetraso());


    }
    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

    private IEnumerator MostrarPostLlamadaConRetraso()
    {
        yield return new WaitForSeconds(2f);
        notificacionPostLlamadaUI.MostrarNotificacion();
    } 

}
