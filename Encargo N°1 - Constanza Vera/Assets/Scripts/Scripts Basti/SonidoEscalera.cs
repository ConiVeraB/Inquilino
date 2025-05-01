using UnityEngine;
using UnityEngine.TextCore.Text;

public class SonidoEscalera : MonoBehaviour
{
    public AudioClip storepieDer;
    public AudioClip storepieIz;

    public AudioClip escaleraIz;
    public AudioClip escaleraDer;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            Character playerScript = other.GetComponent<Character>();

        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            Character playerScript = other.GetComponent<Character>();

        }
    }
}
