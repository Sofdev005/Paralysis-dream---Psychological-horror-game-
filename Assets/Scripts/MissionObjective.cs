using UnityEngine;

// This attribute allows the class to be used as a serializable structure 
// within a list in the Unity Inspector.
[System.Serializable]
public class MissionObjective
{
    [Header("Display")]
    public string objectiveDescription = "Find the missing artifact.";

    [Tooltip("If checked, this objective is tracked by amount (0/3).")]
    public bool usesCount = true;

    [Header("Count Tracking")]
    [Tooltip("The total number required to complete this objective.")]
    public int requiredAmount = 1;

    // --- Runtime Data (Hidden in Inspector) ---
    [HideInInspector]
    public int currentAmount = 0;

    [HideInInspector]
    public bool isCompleted = false;
}