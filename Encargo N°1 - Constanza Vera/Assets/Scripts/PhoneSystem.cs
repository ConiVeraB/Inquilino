using UnityEngine;

public class PhoneSystem : MonoBehaviour
{
    public GameObject Phone;
    void Start()
    {
        Phone.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (Phone != null)
            {
                Phone.SetActive(true);
            }
            
        }
       
    }
}
