using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Puntos de patrulla")]
    public Transform[] puntos;

    [Header("Tiempo de espera")]
    public float espera = 2f;

    [Header("Audio de pasos")]
    public AudioSource audioPasos;

    private NavMeshAgent agent;
    private int indiceActual = 0;
    private float temporizador = 0f;
    private bool enPatrulla = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void IniciarPatrulla()
    {
        if (puntos == null || puntos.Length == 0)
        {
            Debug.LogWarning("Sin puntos de patrulla.");
            return;
        }

        enPatrulla = true;
        indiceActual = 0;
        agent.SetDestination(puntos[indiceActual].position);
    }

    public void DetenerPatrulla()
    {
        enPatrulla = false;
        agent.ResetPath();
        if (audioPasos != null) audioPasos.Stop();
    }

    void Update()
    {
        if (!enPatrulla || puntos.Length == 0 || !agent.isOnNavMesh) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            temporizador += Time.deltaTime;

            if (temporizador >= espera)
            {
                indiceActual++;
                if (indiceActual >= puntos.Length)
                {
                    Debug.Log("Sujeto terminó la patrulla y desaparece.");
                    GetComponent<EnemyController>()?.DesactivarSujeto();
                    return;
                }

                agent.SetDestination(puntos[indiceActual].position);
                temporizador = 0f;
            }
        }

        if (audioPasos != null)
        {
            if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
                if (!audioPasos.isPlaying) audioPasos.Play();
                else if (audioPasos.isPlaying) audioPasos.Stop();
        }
    }
}




