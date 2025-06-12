using UnityEngine;

public class LuzParpadeante : MonoBehaviour
{
    public Light luz;
    public float velocidadParpadeo = 0.2f;
    public float duracionParpadeo = 5f; 
    private float tiempoRestante = 0f;
    private float timer = 0f;
    private bool parpadeando = false;

    void Start()
    {
        if (luz == null)
            luz = GetComponent<Light>();
    }

    void Update()
    {
        if (!parpadeando || luz == null) return;

        timer += Time.deltaTime;
        tiempoRestante -= Time.deltaTime;

        if (timer >= velocidadParpadeo)
        {
            luz.enabled = !luz.enabled;
            timer = 0f;
        }

        if (tiempoRestante <= 0f)
        {
            DetenerParpadeo();
        }
    }

    public void IniciarParpadeo()
    {
        parpadeando = true;
        tiempoRestante = duracionParpadeo;
        timer = 0f;
    }

    public void DetenerParpadeo()
    {
        parpadeando = false;
        if (luz != null)
            luz.enabled = true; 
    }
}

