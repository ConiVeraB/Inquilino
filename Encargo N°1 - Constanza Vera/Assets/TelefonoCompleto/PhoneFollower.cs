using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Target")] 
    public Transform playerTransform; 

    [Header("Offsets")]
    public Vector3 positionOffset;    
    public Vector3 rotationOffset;    

    [Header("Appearance")]
    public Vector3 desiredScale = new Vector3(0.1f, 0.1f, 0.1f);    

    public bool followEnabled = true; 


    void LateUpdate()
    {
        if (!followEnabled || playerTransform == null)
            return;

        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform no asignado en el script FollowPlayer en el objeto: " + gameObject.name);
            return; 
        }

      
        Vector3 targetPosition = playerTransform.position +
                                 playerTransform.right * positionOffset.x +   
                                 playerTransform.up * positionOffset.y +      
                                 playerTransform.forward * positionOffset.z; 

        
        Quaternion targetRotation = playerTransform.rotation * Quaternion.Euler(rotationOffset);

       
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        
    }

    void OnEnable()
    {
        transform.localScale = desiredScale; 
    }

}



