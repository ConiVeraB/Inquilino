using UnityEngine;
using UnityEngine.UI;

public class ControlsOnScreen : MonoBehaviour
{
    public float timeOnScreen = 5f;
    public GameObject controls;
    void Start()
    {
        controls.SetActive(true);
    }
   
}
