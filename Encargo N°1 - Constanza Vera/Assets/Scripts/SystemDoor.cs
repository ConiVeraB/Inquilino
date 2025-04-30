using UnityEngine;

public class SystemDoor : MonoBehaviour
{

    public bool doorOpen = false;
    public float doorOpenAngle = -45f;
    public float doorCloseAngle = -89.98f;
    public float smooth = 3.0f;

    void Update()
    {
        if(doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(-90f, doorOpenAngle, 0f);
            transform.localRotation=Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(-89.98f, doorCloseAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }
}
