using UnityEngine;
using UnityEngine.UI;

public class LlamadaCelular : MonoBehaviour
{
    public AudioSource audioSource;
    //public Button stopButton;
    private bool hasPlayed = false;

    private void Start()
    {
        //stopButton.onClick.AddListener(StopAudio);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }

    private void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            
        }
    }
}
