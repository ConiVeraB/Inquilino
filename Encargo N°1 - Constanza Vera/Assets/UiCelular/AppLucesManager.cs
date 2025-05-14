using UnityEngine;
using UnityEngine.UI;

public class AppLucesManager : MonoBehaviour
{
    [Header("Luces de la escena")]
    public GameObject[] luces; 

    [Header("Toggles de la UI")]
    public Toggle[] toggles;

    void Start()
    {
        
        for (int i = 0; i < luces.Length && i < toggles.Length; i++)
        {
            int index = i; 
            bool estado = luces[i].activeSelf;

            toggles[i].isOn = estado;

            
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

