using UnityEngine;

public class Vocals : MonoBehaviour
{
    private AudioSource source;
    public static Vocals instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Say(AudioObject clip)
    {
        if (clip.clip)
        {
            if (source.isPlaying)
                source.Stop();
            source.PlayOneShot(clip.clip);
        }

        Subtitulo.instance.SetSubtitle(clip.subtitle, clip.clip.length);
    }

        
}
