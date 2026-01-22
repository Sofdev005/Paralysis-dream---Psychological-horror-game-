using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro; // Used for efficient text building

public class QuestLogManager : MonoBehaviour
{
    [Header("Objectives List")]
    // The list of all objectives the player needs to complete
    [SerializeField] public MissionObjective[] currentQuestObjectives;

    [Header("UI Reference")]
    // Assign the main Text UI component that will display the entire list
    [SerializeField]public TextMeshProUGUI questLogText;

    [Header("Formatting")]
    // This prefix is used for tasks that are NOT completed
    [SerializeField]public string activePrefix = "- ";

    // --- Initialization ---

    void Start()
    {
        if (questLogText == null)
        {
            Debug.LogError("QuestLogManager requires a Text component reference in the Inspector!");
            return;
        }

        UpdateQuestLogUI();
    }

    // --- Core Logic: Call this function from your interaction scripts! ---

    // Use the objective's description text as a unique identifier to update its progress.
    public void UpdateObjectiveProgress(string objectiveDescriptionToMatch, int amount = 1)
    {
        for (int i = 0; i < currentQuestObjectives.Length; i++)
        {
            MissionObjective objective = currentQuestObjectives[i];

            // Check if the objective matches the required description (case-insensitive)
            if (objective.objectiveDescription.Equals(objectiveDescriptionToMatch, System.StringComparison.OrdinalIgnoreCase))
            {
                if (objective.isCompleted) return; // Ignore updates for completed tasks

                // Update the count and clamp it
                objective.currentAmount += amount;
                objective.currentAmount = Mathf.Clamp(objective.currentAmount, 0, objective.requiredAmount);

                // Check for completion status
                if (objective.currentAmount >= objective.requiredAmount)
                {
                    objective.isCompleted = true;
                    Debug.Log($"Objective Completed: {objective.objectiveDescription}");
                }

                // Refresh the UI display to show the update (or removal)
                UpdateQuestLogUI();
                return; // Stop searching once the objective is found and updated
            }
        }
        Debug.LogWarning($"Objective not found: {objectiveDescriptionToMatch}");
    }

    // --- UI Update Function ---

    public void UpdateQuestLogUI()
    {
        // StringBuilder is used to efficiently create the final string for the Text component
        StringBuilder sb = new StringBuilder();
        int taskNumber = 1; // Used for the "1-", "2-" numbering

        foreach (MissionObjective objective in currentQuestObjectives)
        {
            // NEW: Skip (do not display) any objective that is completed
            if (objective.isCompleted)
            {
                continue;
            }

            // 1. Determine Prefix (Active prefix is used for all remaining tasks)
            string prefix = activePrefix;

            // 2. Build the Progress Status String (only if it uses a count)
            string progressStatus = "";
            if (objective.usesCount)
            {
                // Format: (0/3)
                progressStatus = $" ({objective.currentAmount}/{objective.requiredAmount})";
            }

            // 3. Combine and Append to the Log in the desired format
            // Example: — 1- Find the 3 anomalies (0/3)
            sb.AppendLine($"{prefix}{taskNumber}- {objective.objectiveDescription}{progressStatus}");

            taskNumber++;
        }

        questLogText.text = sb.ToString();
    }
}