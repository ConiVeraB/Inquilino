using UnityEngine;
using System.Collections;

public class AutosAudios : MonoBehaviour
{
    public AudioSource auto1;
    public AudioSource auto2;
    public float delayBetween = 2f; // Tiempo entre cada sonido

    private void Start()
    {
        StartCoroutine(PlayAlternatingSounds());
    }

    IEnumerator PlayAlternatingSounds()
    {
        while (true)
        {
            if (auto1 != null)
            {
                auto1.Play();
                yield return new WaitForSeconds(auto1.clip.length + delayBetween);
            }

            if (auto2 != null)
            {
                auto2.Play();
                yield return new WaitForSeconds(auto2.clip.length + delayBetween);
            }
        }
    }
}
