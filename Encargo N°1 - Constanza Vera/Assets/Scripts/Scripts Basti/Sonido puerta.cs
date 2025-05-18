using UnityEngine;

public class Sonidopuerta : MonoBehaviour
{
    [Header("Script original que tiene SetOpenDoor")]
    public MonoBehaviour scriptOriginalPuerta;

    private System.Reflection.MethodInfo metodoAbrirCerrar;
    private AudioSource audioPuerta;

    void Start()
    {
        if (scriptOriginalPuerta == null)
        {
            Debug.LogError("No se asignó el script original de la puerta.");
            enabled = false;
            return;
        }

        // Accedemos al método SetOpenDoor
        metodoAbrirCerrar = scriptOriginalPuerta.GetType().GetMethod("SetOpenDoor");

        // Accedemos al AudioSource 'puerta' del script original
        var campoAudio = scriptOriginalPuerta.GetType().GetField("puerta");
        if (campoAudio != null)
        {
            audioPuerta = campoAudio.GetValue(scriptOriginalPuerta) as AudioSource;
        }
    }

    void Update()
    {
        // Detectar solo cuando se presiona la tecla E una vez
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool teclaMantenida = Input.GetKey(KeyCode.E);

            // Si se mantiene presionada, silenciamos temporalmente
            if (teclaMantenida && audioPuerta != null)
            {
                audioPuerta.mute = true;
            }

            // Llamar a SetOpenDoor del script original
            metodoAbrirCerrar?.Invoke(scriptOriginalPuerta, null);

            // Restauramos el audio
            if (audioPuerta != null)
            {
                audioPuerta.mute = false;
            }
        }
    }
}
