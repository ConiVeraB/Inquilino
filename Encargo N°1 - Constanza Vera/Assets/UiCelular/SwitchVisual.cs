using UnityEngine;
using UnityEngine.UI;

public class SwitchVisual : MonoBehaviour
{
    public Sprite switchOn;
    public Sprite switchOff;

    private Toggle toggle;
    private Image image;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        image = GetComponentInChildren<Image>();
    }

    void Start()
    {
        if (toggle != null)
        {
            UpdateVisual(toggle.isOn);
            toggle.onValueChanged.AddListener(UpdateVisual);
        }
    }

    public void UpdateVisual(bool isOn)
    {
        if (image != null)
        {
            image.sprite = isOn ? switchOn : switchOff;
        }
    }

    public void ForceUpdate(bool isOn)
    {
        UpdateVisual(isOn);
    }
}

