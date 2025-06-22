using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EmergencyCall : MonoBehaviour
{
    public GameObject emergencyPanel;
    public bool hasEnoughEvidence = false;

    public AudioSource noEvidenceAudioSource;
    public AudioSource evidenceAudioSource;

    public Button[] numberButtons;
    public Button callButton;

    // AÑADE ESTA LÍNEA para referenciar tu componente de texto de UI
    public TextMeshProUGUI displayNumberText; // Cambia a 'public Text displayNumberText;' si usas el Text UI Legacy

    private string currentNumber = "";

    void Start()
    {
        if (emergencyPanel != null)
        {
            emergencyPanel.SetActive(false);
        }

        SetCursorState(false);

        // Inicializa el texto del display
        if (displayNumberText != null)
        {
            displayNumberText.text = ""; // Asegúrate de que esté vacío al inicio
        }

        foreach (Button btn in numberButtons)
        {
            int number;
            if (int.TryParse(btn.GetComponentInChildren<Text>().text, out number) || // Para Text UI Legacy
                (btn.GetComponentInChildren<TextMeshProUGUI>() != null && int.TryParse(btn.GetComponentInChildren<TextMeshProUGUI>().text, out number))) // Para TextMeshPro
            {
                int numToPass = number;
                btn.onClick.AddListener(() => AddNumberToDial(numToPass.ToString()));
            }
            else
            {
                Debug.LogWarning("El botón " + btn.name + " no tiene un texto numérico válido.");
            }
        }

        if (callButton != null)
        {
            callButton.onClick.AddListener(AttemptCall);
        }
        else
        {
            Debug.LogWarning("El botón de llamar no está asignado.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            bool panelActive = !emergencyPanel.activeSelf;
            emergencyPanel.SetActive(panelActive);
            SetCursorState(panelActive);

            // Reinicia el número y el display cada vez que abres el panel
            if (panelActive)
            {
                currentNumber = "";
                if (displayNumberText != null)
                {
                    displayNumberText.text = currentNumber;
                }
            }
        }
    }

    void SetCursorState(bool activateCursor)
    {
        if (activateCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Cursor activado.");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Cursor desactivado.");
        }
    }

    public void AddNumberToDial(string number)
    {
        if (currentNumber.Length < 9) // Limita la longitud del número (ej. para 911 o más dígitos)
        {
            currentNumber += number;
            Debug.Log("Número marcado: " + currentNumber);

            // AÑADE ESTAS LÍNEAS para actualizar el texto en la UI
            if (displayNumberText != null)
            {
                displayNumberText.text = currentNumber;
            }
        }
    }

    public void AttemptCall()
    {
        if (currentNumber == "911")
        {
            if (hasEnoughEvidence)
            {
                if (evidenceAudioSource != null)
                {
                    evidenceAudioSource.Play();
                    Debug.Log("Llamada exitosa a 911. Reproduciendo audio con pruebas.");
                }
            }
            else
            {
                if (noEvidenceAudioSource != null)
                {
                    noEvidenceAudioSource.Play();
                    Debug.Log("No hay pruebas suficientes para llamar al 911. Reproduciendo audio sin pruebas.");
                }
            }
            // Después de la llamada, reinicia el número y el display
            currentNumber = "";
            if (displayNumberText != null)
            {
                displayNumberText.text = currentNumber;
            }
            // Opcional: podrías querer desactivar el panel después de la llamada
            // emergencyPanel.SetActive(false);
            // SetCursorState(false);
        }
        else
        {
            Debug.Log("Número marcado no válido: " + currentNumber + ". Debe ser 911.");
            // Opcional: limpiar el número o mostrar un mensaje de error en la UI
            currentNumber = "";
            if (displayNumberText != null)
            {
                displayNumberText.text = "Número Inválido"; // O un mensaje de error temporal
            }
        }
    }

    public void SetEvidenceStatus(bool status)
    {
        hasEnoughEvidence = status;
        Debug.Log("Estado de las pruebas cambiado a: " + hasEnoughEvidence);
    }
}
