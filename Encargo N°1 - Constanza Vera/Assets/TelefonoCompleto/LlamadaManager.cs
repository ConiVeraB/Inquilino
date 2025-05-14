using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DatosLlamada
{
    public string nombre;
    public Sprite imagenContacto;
    public float tiempoInicio; // En segundos desde el inicio del juego
    public GameObject panelLlamada; // Panel de llamada entrante
    public GameObject panelEnLlamada; // Panel durante la llamada
    public float duracionMaxima = 10f; // Duración de la llamada en segundos
    public AudioClip audioConversacion; // Audio único de esta llamada
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
    public AudioSource audioLlamada;      // Audio del tono de llamada entrante
    public AudioClip clipLlamada;

    [Header("Audio conversación")]
    public AudioSource audioConversacion; // Audio para reproducir durante la llamada

    [Header("Vibración")]
    public RectTransform imagenAVibrar;
    public float intensidadVibracion = 10f;
    public float frecuenciaVibracion = 25f;

    private float tiempoJuego;
    private int indiceActual = 0;
    private bool llamadaEnCurso = false;
    private Coroutine llamadaActiva;
    private bool vibrando = false;
    private Vector3 posicionOriginal;

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
        botonCortar.onClick.AddListener(() => CortarLlamada(llamada));

        botonCortarEnLlamada.onClick.RemoveAllListeners();
        botonCortarEnLlamada.onClick.AddListener(() => CortarLlamada(llamada));
    }

    void ContestarLlamada(DatosLlamada llamada)
    {
        llamada.panelLlamada?.SetActive(false);
        llamada.panelEnLlamada?.SetActive(true);

        if (audioLlamada != null && audioLlamada.isPlaying)
            audioLlamada.Stop();

        if (vibrando && imagenAVibrar != null)
        {
            imagenAVibrar.anchoredPosition = posicionOriginal;
            vibrando = false;
        }

        if (textoDuracionLlamada != null)
            textoDuracionLlamada.text = "00:00";

        // Oculta botón de cortar
        botonCortarEnLlamada.interactable = false;
        botonCortarEnLlamada.gameObject.SetActive(false);

        llamadaActiva = StartCoroutine(ContarDuracionLlamada(llamada));

        // Inicia el audio único de la llamada
        if (audioConversacion != null && llamada.audioConversacion != null)
        {
            audioConversacion.clip = llamada.audioConversacion;
            audioConversacion.Play();
            StartCoroutine(EsperarFinConversacion(llamada.audioConversacion.length, llamada));
        }
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
    }

    IEnumerator EsperarFinConversacion(float duracion, DatosLlamada llamada)
    {
        yield return new WaitForSeconds(duracion);

        // Finaliza automáticamente la llamada
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

        FinalizarLlamada(llamada);
    }

    void FinalizarLlamada(DatosLlamada llamada)
    {
        llamada.panelLlamada?.SetActive(false);
        llamada.panelEnLlamada?.SetActive(false);

        indiceActual++;
        llamadaEnCurso = false;
    }
}
