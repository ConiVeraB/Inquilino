using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class ZonaPresencia : MonoBehaviour
{
    [System.Serializable]
    public class SonidoPresencia
    {
        public AudioClip clip;
        public bool soloUnaVez;
        [HideInInspector] public bool yaActivado = false;

        [Header("Efectos visuales")]
        public GameObject objetoAActivar;
        public bool activarTemporalmente = false;
        public float duracionActivacion = 1.5f;

        public Animator animador;
        public string triggerAnimacion;

        [Header("Parpadeo de luces")]
        public List<Light> lucesAParpadear;
        public bool usarFlickerIntenso = false;
        public int cantidadParpadeos = 5;
        public float intensidadMin = 0.2f;
        public float intensidadMax = 2.5f;
        public float velocidadFlicker = 0.05f;

        [Header("Efectos atmosféricos")]
        public GameObject niebla;
        public bool activarNieblaTemporal = false;
        public float duracionNiebla = 3f;

        public Volume perfilPostProcesado;
        public bool usarDistorsionVisual = false;
        public float intensidadDistorsion = 0.5f;
        public float duracionDistorsion = 2f;

        [Header("Cordura")]
        public float danoCordura = 0f;


    }

    [Header("Configuración de presencia")]
    public SonidoPresencia[] sonidosPresencia;
    public bool usarOrdenSecuencial = true;
    public bool reiniciarSecuencia = false;

    public float tiempoEntreEventos = 8f;
    public float distanciaActivacion = 6f;
    public float anguloDeEspalda = 100f;

    [Header("Referencia al jugador")]
    public Transform jugador;
    public Transform cabezaJugador;

    private float tiempoActual = 0f;
    private bool jugadorDentro = false;
    private int sonidoActualIndex = 0;

    void Update()
    {
        if (!jugadorDentro || sonidosPresencia.Length == 0)
            return;

        tiempoActual += Time.deltaTime;

        if (tiempoActual >= tiempoEntreEventos && EstaDeEspaldas())
        {
            ReproducirSonidoConEfecto();
            tiempoActual = 0f;
        }
    }

    private bool EstaDeEspaldas()
    {
        Vector3 direccionJugador = cabezaJugador.forward;
        Vector3 direccionZona = (transform.position - cabezaJugador.position).normalized;

        float angulo = Vector3.Angle(direccionJugador, direccionZona);
        return angulo > anguloDeEspalda;
    }

    private void ReproducirSonidoConEfecto()
    {
        SonidoPresencia sonido = ObtenerSiguienteSonido();
        if (sonido == null) return;

        AudioSource.PlayClipAtPoint(sonido.clip, transform.position);
        Debug.Log("Sonido activado: " + sonido.clip.name);

        if (sonido.soloUnaVez)
            sonido.yaActivado = true;

        if (sonido.danoCordura > 0f)
            CorduraManager.instancia?.RestarCordura(sonido.danoCordura);




        // Activar objeto
        if (sonido.objetoAActivar != null)
        {
            if (sonido.activarTemporalmente)
                StartCoroutine(ActivarTemporalmente(sonido.objetoAActivar, sonido.duracionActivacion));
            else
                sonido.objetoAActivar.SetActive(true);
        }

        // Animación
        if (sonido.animador != null && !string.IsNullOrEmpty(sonido.triggerAnimacion))
            sonido.animador.SetTrigger(sonido.triggerAnimacion);

        // Luces
        if (sonido.lucesAParpadear != null && sonido.lucesAParpadear.Count > 0)
        {
            foreach (var luz in sonido.lucesAParpadear)
            {
                if (luz == null) continue;

                if (sonido.usarFlickerIntenso)
                    StartCoroutine(FlickerIntenso(luz, sonido));
                else
                    StartCoroutine(ParpadearLuzSimple(luz, sonido.intensidadMax, sonido.velocidadFlicker));
            }
        }

        // Niebla
        if (sonido.niebla != null)
        {
            if (sonido.activarNieblaTemporal)
                StartCoroutine(ActivarTemporalmente(sonido.niebla, sonido.duracionNiebla));
            else
                sonido.niebla.SetActive(true);
        }

        // Distorsión
        if (sonido.usarDistorsionVisual && sonido.perfilPostProcesado != null)
        {
            StartCoroutine(AplicarDistorsionVisual(sonido.perfilPostProcesado, sonido.intensidadDistorsion, sonido.duracionDistorsion));
        }

      

    }

    private SonidoPresencia ObtenerSiguienteSonido()
    {
        if (usarOrdenSecuencial)
        {
            while (sonidoActualIndex < sonidosPresencia.Length)
            {
                var s = sonidosPresencia[sonidoActualIndex];
                sonidoActualIndex++;

                if (!s.soloUnaVez || !s.yaActivado)
                    return s;
            }

            if (reiniciarSecuencia)
            {
                sonidoActualIndex = 0;
                return ObtenerSiguienteSonido();
            }

            return null;
        }
        else
        {
            List<SonidoPresencia> disponibles = new List<SonidoPresencia>();

            foreach (var s in sonidosPresencia)
            {
                if (!s.soloUnaVez || !s.yaActivado)
                    disponibles.Add(s);
            }

            if (disponibles.Count == 0) return null;

            return disponibles[Random.Range(0, disponibles.Count)];
        }
    }

    private IEnumerator ActivarTemporalmente(GameObject obj, float duracion)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(duracion);
        obj.SetActive(false);
    }

    private IEnumerator ParpadearLuzSimple(Light luz, float intensidad, float duracion)
    {
        float original = luz.intensity;
        luz.intensity = intensidad;
        yield return new WaitForSeconds(duracion);
        luz.intensity = original;
    }

    private IEnumerator FlickerIntenso(Light luz, SonidoPresencia config)
    {
        float original = luz.intensity;

        for (int i = 0; i < config.cantidadParpadeos; i++)
        {
            luz.intensity = Random.Range(config.intensidadMin, config.intensidadMax);
            yield return new WaitForSeconds(config.velocidadFlicker);
        }

        luz.intensity = original;
    }

    private IEnumerator AplicarDistorsionVisual(Volume volume, float intensidad, float duracion)
    {
        LensDistortion distortion;
        ChromaticAberration aberration;

        if (volume.profile.TryGet(out distortion))
        {
            distortion.active = true;
            distortion.intensity.Override(intensidad);
        }

        if (volume.profile.TryGet(out aberration))
        {
            aberration.active = true;
            aberration.intensity.Override(intensidad);
        }

        yield return new WaitForSeconds(duracion);

        if (distortion != null)
        {
            distortion.intensity.Override(0f);
            distortion.active = false;
        }

        if (aberration != null)
        {
            aberration.intensity.Override(0f);
            aberration.active = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;
            tiempoActual = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }
}


