using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movimiento//
    private float HorizontalInput;
    private float VerticalInput;
    public float speed;
    Rigidbody rb;

    //camara//
    private Camera camara;
    private float rotaciónY;

    public float sensibilidadMouse = 2f; // Sensibilidad del mouse
    public float suavidad = 2f;  //Factor de suavizado

    private float rotaciónYActual; // Valor suavizado de la rotación vertical

    public DialogueManager dialogueManager;
    public ObjetiveManager objetiveManager;

    public AudioSource bañoruido;

    public PhoneSystem phoneSystem;

    bool enEscalera = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main;

        rotaciónY = 0;
        rotaciónYActual = 0; // Inicializar la rotación suavizada

        transform.rotation = Quaternion.Euler(0, 0, 0); // Rotación del personaje
        camara.transform.localRotation = Quaternion.Euler(0, 0, 0);

        objetiveManager.AddObjective("Contesta la llamada y enciende la luz al entrar", "Luego ve a la cocina a prepararte un café");
        objetiveManager.UpdateObjective();
    }


    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal") * speed;
        VerticalInput = Input.GetAxis("Vertical") * speed;
        MovimientoCamara();
    }

    void FixedUpdate()
    {
        /*Vector3 forward = camara.transform.forward * VerticalInput;
        Vector3 right = camara.transform.right * HorizontalInput;
        Vector3 movimiento = (forward + right) * Time.fixedDeltaTime;  // Escala por el tiempo fijo
        rb.MovePosition(transform.position + movimiento);*/

        Vector3 camForward = camara.transform.forward;
        camForward.y = 0;
        camForward.Normalize(); // vuelve a normalizar el vector para que no pierda dirección

        Vector3 camRight = camara.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 movimiento = (camForward * VerticalInput + camRight * HorizontalInput) * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + movimiento);

    }

    void MovimientoCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Rotar el personaje en el eje horizontal (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotar la cámara en el eje vertical (Pitch)
        rotaciónY -= mouseY;
        rotaciónY = Mathf.Clamp(rotaciónY, -60, 60);

        //Aplicar Suavizado
        rotaciónYActual = Mathf.Lerp(rotaciónYActual, rotaciónY, suavidad * Time.deltaTime);

        camara.transform.localRotation = Quaternion.Euler(rotaciónYActual, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto con el que el jugador colisionó tiene el tag "Cocina"
        if (other.CompareTag("Cocina"))
        {
            // Muestra el diálogo específico cuando entra a la cocina
            dialogueManager.lines = new string[]
            {
                "Qué raro… Estoy segura que lo dejé aquí"
            };
            dialogueManager.StartDialogue(); // Inicia el diálogo

            objetiveManager.CompleteObjective(); // Completa el objetivo de ir a la cocina
         
            objetiveManager.AddObjective("Busca el tarro de café", "No debió ir muy lejos, ¿verdad?");
            objetiveManager.UpdateObjective();

            if (other.CompareTag("Escalera"))
            {
                enEscalera = true;
                rb.useGravity = false; // Para que no caiga al subir
            }
        }
        if (other.CompareTag("Baño"))
        {
            // Muestra el diálogo específico cuando entra al baño
            bañoruido.Play();
            dialogueManager.lines = new string[]
             {
                "No espero ninguna visita.", // Primera línea
                "¿Quién será a esta hora?"       // Segunda línea
             };
            dialogueManager.StartDialogue(); // Inicia el diálogo

            objetiveManager.CompleteObjective(); // Completa el objetivo de ir al baño
            objetiveManager.AddObjective("Ir a la puerta principal", "Dirígete a la entrada de la casa.");
            objetiveManager.UpdateObjective();

            if (phoneSystem != null)
            {
                phoneSystem.ActivarImagen();
            }

        }
    }

void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Escalera"))
    {
        enEscalera = false;
        rb.useGravity = true; // Vuelve a caer normalmente
    }
}


}