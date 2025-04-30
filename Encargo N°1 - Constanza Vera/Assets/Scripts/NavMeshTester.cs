using UnityEngine;
using UnityEngine.AI;

public class NavMeshTester : MonoBehaviour
{
    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError(" No hay NavMeshAgent en este objeto.");
            return;
        }

        if (agent.isOnNavMesh)
        {
            Debug.Log(" El agente ESTÁ sobre el NavMesh.");
            Vector3 destino = transform.position + transform.forward * 5f;
            agent.SetDestination(destino);
        }
        else
        {
            Debug.LogError("El agente NO está sobre el NavMesh.");
        }
    }
}
