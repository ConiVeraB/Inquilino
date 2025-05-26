using UnityEngine;

public class UIController : MonoBehaviour
{
    public CanvasGroup dialogoGroup;
    public void MostrarDialogo()
    {
        dialogoGroup.alpha = 1;
        dialogoGroup.interactable = true;
        dialogoGroup.blocksRaycasts = true;

        // Opcional: Asegura que esté al frente si está en el mismo Canvas
        dialogoGroup.transform.SetAsLastSibling();
    }

    public void OcultarDialogo()
    {
        dialogoGroup.alpha = 0;
        dialogoGroup.interactable = false;
        dialogoGroup.blocksRaycasts = false;
    }
}
