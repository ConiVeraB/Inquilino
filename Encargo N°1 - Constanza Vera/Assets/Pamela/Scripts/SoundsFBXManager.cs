using UnityEngine;

public class SoundsFBXManager : MonoBehaviour
{
    public static SoundsFBXManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    void Start()// Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        
    }

    void Update()// Update is called once per frame
    {
        
    }
}
