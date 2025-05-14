using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorduraManager : MonoBehaviour
{
    [Header("Valores de cordura")]
    [Range(0, 100)]
    public float cordura = 100f;
    public float maxCordura = 100f;
    public float minCordura = 0f;

    [Header("Degradación pasiva")]
    public bool degradarConTiempo = true;
    public float velocidadDegradacion = 1f;

    [Header("Efectos Visuales")]
    public Volume perfilVisual;
    public float intensidadMaxDistorsion = 0.5f;
    public float intensidadMaxAberracion = 0.4f;

    [Header("Sonidos mentales")]
    public AudioSource fuenteSonidosInternos;
    public AudioClip[] sonidosMentales;
    public float tiempoEntreSonidos = 15f;

    private float timerSonido = 0f;

    private LensDistortion distorsion;
    private ChromaticAberration aberracion;

    public static CorduraManager instancia; // Acceso global

    private void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (perfilVisual != null)
        {
            perfilVisual.profile.TryGet(out distorsion);
            perfilVisual.profile.TryGet(out aberracion);
        }
    }

    private void Update()
    {
        if (degradarConTiempo)
        {
            RestarCordura(Time.deltaTime * velocidadDegradacion);
        }

        AplicarEfectosVisuales();

        timerSonido += Time.deltaTime;
        if (timerSonido >= tiempoEntreSonidos && cordura <= 70f)
        {
            ReproducirSonidoMental();
            timerSonido = 0f;
        }
    }

    public void RestarCordura(float cantidad)
    {
        cordura = Mathf.Clamp(cordura - cantidad, minCordura, maxCordura);
    }

    public void RecuperarCordura(float cantidad)
    {
        cordura = Mathf.Clamp(cordura + cantidad, minCordura, maxCordura);
    }

    private void AplicarEfectosVisuales()
    {
        if (distorsion != null)
        {
            float factor = 1 - (cordura / maxCordura);
            distorsion.intensity.Override(factor * intensidadMaxDistorsion);
            distorsion.active = factor > 0.05f;
        }

        if (aberracion != null)
        {
            float factor = 1 - (cordura / maxCordura);
            aberracion.intensity.Override(factor * intensidadMaxAberracion);
            aberracion.active = factor > 0.05f;
        }
    }

    private void ReproducirSonidoMental()
    {
        if (fuenteSonidosInternos == null || sonidosMentales.Length == 0) return;

        if (!fuenteSonidosInternos.isPlaying)
        {
            var clip = sonidosMentales[Random.Range(0, sonidosMentales.Length)];
            fuenteSonidosInternos.PlayOneShot(clip);
        }
    }
}
