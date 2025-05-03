using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Puntos de patrulla")]
    public Transform[] patrolPoints;

    [Header("Tiempo de espera entre puntos")]
    public float waitTime = 2f;

    [Header("Audio")]
    public AudioSource footstepAudio;
   
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Intentamos colocar el destino después de un frame, cuando esté sobre el NavMesh
        StartCoroutine(EsperarYAsignarDestino());
    }

    System.Collections.IEnumerator EsperarYAsignarDestino()
    {
        // Esperar un frame completo
        yield return null;

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("El enemigo NO está sobre el NavMesh.");
            yield break;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("No hay puntos de patrulla asignados.");
            yield break;
        }

        agent.SetDestination(patrolPoints[0].position);
        Debug.Log("Patrullaje iniciado hacia: " + patrolPoints[0].position);
    }


    void Update()
    {
        if (agent == null || !agent.isOnNavMesh || patrolPoints == null || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                // Si está en el último punto, desaparecer
                if (currentPointIndex == patrolPoints.Length - 1)
                {
                    Debug.Log("El Sujeto llegó al punto final y desaparece.");
                    gameObject.SetActive(false); // Desaparece
                    return;
                }

                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPointIndex].position);
                waitTimer = 0f;
            }
        }
        if (footstepAudio != null)
        {
            if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
            {
                if (!footstepAudio.isPlaying)
                {
                    footstepAudio.Play();
                }
            }
            else
            {
                if (footstepAudio.isPlaying)
                {
                    footstepAudio.Stop();
                }
            }
        }
    }
}



