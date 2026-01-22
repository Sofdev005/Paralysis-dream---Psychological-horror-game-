using UnityEngine;

/// <summary>
/// Singleton to manage global game state, tracking collected items.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Quest Goal")]
    [SerializeField] private int requiredItemsToUnlockKey = 3;
    private int _itemsCollected = 0;

    [Header("Key Spawn Settings")]
    [Tooltip("The KeyItem GameObject prefab or object to activate.")]
    [SerializeField] private GameObject keyObject;
    [SerializeField] private Transform keySpawnLocation;

    // --- NEW QUEST LOG INTEGRATION ---
    private QuestLogManager questManager;
    [Tooltip("The exact description of the 'Find Anomalies' objective.")]
    [SerializeField] private string objectiveDescription = "Find the 3 anomalies";
    // ---------------------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            // Ensure the key is hidden initially
            if (keyObject != null)
            {
                keyObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        // Find the Quest Log Manager once at the start
        questManager = FindObjectOfType<QuestLogManager>();
        if (questManager == null)
        {
            Debug.LogWarning("QuestLogManager not found. Anomaly quest tracking disabled.");
        }
    }

    /// <summary>
    /// Called by CollectableItem scripts when they are picked up.
    /// </summary>
    public void AddCollectedItem()
    {
        _itemsCollected++;
        Debug.Log($"Item collected! Total: {_itemsCollected} / {requiredItemsToUnlockKey}");

        // --- NEW QUEST LOG UPDATE ---
        if (questManager != null)
        {
            // Call the manager to increase the progress by 1
            questManager.UpdateObjectiveProgress(objectiveDescription, 1);
        }
        // ----------------------------

        // Check if the goal has been reached
        if (_itemsCollected >= requiredItemsToUnlockKey)
        {
            SpawnKey();
        }
    }

    private void SpawnKey()
    {

        if (keyObject != null && !keyObject.activeSelf)
        {
            // If the key is a disabled object in the scene:
            keyObject.SetActive(true);

            // Optional: Move it to the specified spawn location
            if (keySpawnLocation != null)
            {
                keyObject.transform.position = keySpawnLocation.position;
                keyObject.transform.rotation = keySpawnLocation.rotation;
            }

            Debug.Log("Quest complete! The Key has appeared.");
        }
        else if (keyObject != null)
        {
            Debug.Log("The Key is already active/spawned.");
        }
    }
}