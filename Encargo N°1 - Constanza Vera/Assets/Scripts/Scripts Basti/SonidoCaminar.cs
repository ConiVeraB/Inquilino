using UnityEngine;

public class SonidoCaminar : MonoBehaviour
{
    public AudioSource pies;
    public AudioClip pieDer;
    public AudioClip pieIz;


    private void FixedUpdate()
    {
        Vector3 m_input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (m_input != Vector3.zero)
        {
            if (!pies.isPlaying)
            {
                if (pies.clip == pieIz)
                {
                    pies.clip = pieDer;
                }
                else if (pies.clip == pieDer)
                {
                    pies.clip = pieIz;
                }
                else
                {
                    pies.clip = pieDer;
                }
                pies.Play();

            }
        }
        
    }
}
