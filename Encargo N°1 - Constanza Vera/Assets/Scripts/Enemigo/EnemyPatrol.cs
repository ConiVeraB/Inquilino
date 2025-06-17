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

    [HideInInspector]
    public bool permitirPatrulla = false;



    private NavMeshAgent agent;
    private int indiceActual = 0;
    private float temporizador = 0f;
    private bool enPatrulla = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enPatrulla = false;
        puntos = new Transform[0]; 
    }


    public void IniciarPatrulla()
    {
        if (!permitirPatrulla)
        {
            Debug.LogWarning($"[{name}] Patrulla bloqueada: aún no está permitido iniciar.");
            return;
        }

        if (puntos == null || puntos.Length == 0)
        {
            Debug.LogWarning($"[{name}] Sin puntos de patrulla asignados.");
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
        if (!enPatrulla || puntos == null || puntos.Length == 0 || !agent.isOnNavMesh)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            
            if (indiceActual < puntos.Length - 1)
            {
                temporizador += Time.deltaTime;

                if (temporizador >= espera)
                {
                    indiceActual++;
                    agent.SetDestination(puntos[indiceActual].position);
                    temporizador = 0f;
                }
            }
            else
            {
              
                Debug.Log($"[{name}] Ruta completada, desactivando enemigo.");
                enPatrulla = false;
                GetComponent<EnemyController>()?.DesactivarSujeto();
            }
        }

        if (audioPasos != null)
        {
            if (agent.velocity.magnitude > 0.1f && agent.remainingDistance > agent.stoppingDistance)
            {
                if (!audioPasos.isPlaying) audioPasos.Play();
            }
            else
            {
                if (audioPasos.isPlaying) audioPasos.Stop();
            }
        }
    }

}




