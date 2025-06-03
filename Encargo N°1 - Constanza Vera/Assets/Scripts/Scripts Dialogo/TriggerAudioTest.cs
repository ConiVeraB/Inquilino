using UnityEngine;

public class TriggerAudioTest : MonoBehaviour
{
    public AudioObject clipToPlay;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Vocals.instance.Say(clipToPlay);
    }
}
