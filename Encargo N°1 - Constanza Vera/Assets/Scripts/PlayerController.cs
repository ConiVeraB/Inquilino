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
        HorizontalInput = Input.GetAxis("Horizontal") * speed;
        VerticalInput = Input.GetAxis("Vertical") * speed;
        MovimientoCamara();
    }

    void FixedUpdate()
    {
        Vector3 forward = camara.transform.forward * VerticalInput;
        Vector3 right = camara.transform.right * HorizontalInput;
        Vector3 movimiento = (forward + right) * Time.fixedDeltaTime;  // Escala por el tiempo fijo
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
}