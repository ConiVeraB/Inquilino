using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjetiveManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveText; // Aquí se mostrará el texto de los objetivos
    public List<Objetives> objectives = new List<Objetives>(); // Lista de los objetivos
    private int currentObjectiveIndex = 0; // Para llevar el seguimiento de cuál es el objetivo actual

    void Start()
    {
        UpdateObjective();
    }

    public void AddObjective(string title, string description)
    {
        Objetives newObjective = new Objetives
        {
            title = title,
            description = description,
            isCompleted = false
        };
        objectives.Add(newObjective);
    }

    // Actualizar el objetivo en la UI
    public void UpdateObjective()
    {
        if (currentObjectiveIndex < objectives.Count)
        {
            Objetives currentObjective = objectives[currentObjectiveIndex];
            objectiveText.text = currentObjective.title + "\n" + currentObjective.description;
        }
        else
        {
            objectiveText.text = ""; // No mostrar nada cuando no haya más objetivos
        }
    }

    // Marcar un objetivo como completado
    public void CompleteObjective()
    {
        if (currentObjectiveIndex < objectives.Count)
        {
            objectives[currentObjectiveIndex].isCompleted = true;
            currentObjectiveIndex++; // Pasar al siguiente objetivo

            if (currentObjectiveIndex < objectives.Count) // Verificar si aún hay objetivos
            {
                UpdateObjective(); // Actualizar la UI con el siguiente objetivo
            }
            else
            {
                objectiveText.text = ""; // No mostrar nada si no hay más objetivos
            }
        }
    }
}
