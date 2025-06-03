using UnityEngine;

public class DialogoTrigger : MonoBehaviour
{
  public Dialogo dialogo;
  
  public void TriggerDialogo()
    {
        FindObjectOfType<DialogoManager>().StartDialogo(dialogo);

            
    }
}
