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

    public bool usingPhone;
    public DialogueManager dialogueManager;
   

    public AudioSource bañoruido;

    public PhoneSystem phoneSystem;

    public GameObject telefono;

   

    bool enEscalera = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main;

        rotaciónY = 0;
        rotaciónYActual = 0; // Inicializar la rotación suavizada

        transform.rotation = Quaternion.Euler(0, 0, 0); // Rotación del personaje
        camara.transform.localRotation = Quaternion.Euler(0, 0, 0);

        
    }


    void Update()
    {
        if (Pausemenu.GameIsPaused) return;

        if (usingPhone)
        {
        HorizontalInput = Input.GetAxis("Horizontal") * speed;
        VerticalInput = Input.GetAxis("Vertical") * speed;
        MovimientoCamara();

        }
    }

    void FixedUpdate()
    {
        /*Vector3 forward = camara.transform.forward * VerticalInput;
        Vector3 right = camara.transform.right * HorizontalInput;
        Vector3 movimiento = (forward + right) * Time.fixedDeltaTime;  // Escala por el tiempo fijo
        rb.MovePosition(transform.position + movimiento);*/
        if (usingPhone)
        {
            Vector3 camForward = camara.transform.forward;
            camForward.y = 0;
            camForward.Normalize(); // vuelve a normalizar el vector para que no pierda dirección

            Vector3 camRight = camara.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 movimiento = (camForward * VerticalInput + camRight * HorizontalInput) * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + movimiento);
        }
    }

    void MovimientoCamara()
    {
        if (Pausemenu.GameIsPaused) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


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

          
            }


       if (other.CompareTag("Escalera"))
       {
          enEscalera = true;
          rb.useGravity = false; // Para que no caiga al subir
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
    //void AgarrarTelefono()
    //{
    //    telefono.transform.SetParent(Player.transform);
    //    telefono.transform.localPosition = new Vector3(0, 1.5f, 1);
    //    telefono.transform.localRotation = Quaternion.identity;
    //}

}
