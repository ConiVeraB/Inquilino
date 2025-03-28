using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Poltergeist : MonoBehaviour
{
    [System.Serializable]
    public struct PosicionRotacion
    {
        public Vector3 posicion;
        public Vector3 rotacion; // Usa Vector3 para ángulos de Euler
    }

    public List<PosicionRotacion> posicionesRotaciones = new List<PosicionRotacion>();
    private int indiceActual = 0;

    void Start()
    {
        if (posicionesRotaciones.Count == 0)
        {
            Debug.LogWarning("Advertencia: No hay posiciones/rotaciones definidas en la lista. Agrega elementos a la lista 'Posiciones Rotaciones' en el Inspector.");
            return;
        }

        transform.position = posicionesRotaciones[0].posicion;
        transform.eulerAngles = posicionesRotaciones[0].rotacion;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MoverASiguientePosicion();
        }
    }

    void MoverASiguientePosicion()
    {
        if (posicionesRotaciones.Count == 0)
        {
            return;
        }

        indiceActual = (indiceActual + 1) % posicionesRotaciones.Count;
        transform.position = posicionesRotaciones[indiceActual].posicion;
        transform.eulerAngles = posicionesRotaciones[indiceActual].rotacion;
    }
}
