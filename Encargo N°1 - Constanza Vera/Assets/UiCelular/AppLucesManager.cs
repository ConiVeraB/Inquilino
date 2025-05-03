using UnityEngine;
using UnityEngine.UI;

public class AppLucesManager : MonoBehaviour
{
    [Header("Luces de la escena")]
    public GameObject[] luces; // Las luces físicas en la escena

    [Header("Toggles de la UI")]
    public Toggle[] toggles; // Los toggles visuales que controla cada luz

    void Start()
    {
        // Asegura sincronización inicial entre luces y switches
        for (int i = 0; i < luces.Length && i < toggles.Length; i++)
        {
            int index = i; // evita el problema de referencia en el loop
            bool estado = luces[i].activeSelf;

            toggles[i].isOn = estado;

            // Se asegura de actualizar visual al comenzar
            var visual = toggles[i].GetComponent<SwitchVisual>();
            if (visual != null) visual.ForceUpdate(estado);

            toggles[i].onValueChanged.AddListener((value) => ToggleLuz(index, value));
        }
    }

    public void ToggleLuz(int index, bool estado)
    {
        if (index >= 0 && index < luces.Length)
        {
            luces[index].SetActive(estado);
        }
    }

    public void EncenderTodas()
    {
        for (int i = 0; i < luces.Length && i < toggles.Length; i++)
        {
            luces[i].SetActive(true);
            toggles[i].isOn = true;
        }
    }

    public void ApagarTodas()
    {
        for (int i = 0; i < luces.Length && i < toggles.Length; i++)
        {
            luces[i].SetActive(false);
            toggles[i].isOn = false;
        }
    }
}

