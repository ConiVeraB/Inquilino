using UnityEngine;
using UnityEngine.UI;

public class PanelDecisionSimple : MonoBehaviour
{
    public Button botonOpcion1;
    public Button botonOpcion2;

    void Start()
    {
        if (botonOpcion1 != null)
            botonOpcion1.onClick.AddListener(() => SeleccionarOpcion("Opción 1"));

        if (botonOpcion2 != null)
            botonOpcion2.onClick.AddListener(() => SeleccionarOpcion("Opción 2"));
    }

    void SeleccionarOpcion(string texto)
    {
        Debug.Log("Elegiste: " + texto);
        gameObject.SetActive(false);
    }
}
