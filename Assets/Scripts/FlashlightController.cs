using UnityEngine;

/// <summary>
/// Manages the equipped flashlight functionality (on/off toggle).
/// </summary>
public class FlashlightController : MonoBehaviour
{
    // --- NEW QUEST LOG REFERENCE ---
    private QuestLogManager questManager;
    [Tooltip("The exact description of the 'Find Flashlight' objective.")]
    [SerializeField] private string objectiveDescription = "Find a flashlight";
    // ---------------------------------

    [Header("Flashlight Settings")]
    [Tooltip("The Light component used as the flashlight beam.")]
    [SerializeField] private Light flashlightLight;
    [Tooltip("The GameObject representing the physical model of the flashlight in the player's hand.")]
    [SerializeField] private GameObject flashlightModel;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private bool _hasFlashlight = false;
    private bool _isFlashlightOn = false;

    void Start()
    {
        // Find the Quest Log Manager once at the start
        questManager = FindObjectOfType<QuestLogManager>();
        if (questManager == null)
        {
            Debug.LogWarning("QuestLogManager not found. Flashlight quest tracking disabled.");
        }

        // Ensure both the light and the model are off/hidden at start.
        if (flashlightLight != null)
            flashlightLight.enabled = false;

        if (flashlightModel != null)
            flashlightModel.SetActive(false);
    }

    void Update()
    {
        // Only allow toggling if the player has the item
        if (_hasFlashlight && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    /// <summary>
    /// Called by the FlashlightItem when the player picks it up.
    /// </summary>
    public void PickupFlashlight()
    {
        _hasFlashlight = true;

        // Show the model in the player's hand
        if (flashlightModel != null)
            flashlightModel.SetActive(true);

        // Turn it on immediately upon pickup
        if (flashlightLight != null)
        {
            _isFlashlightOn = true;
            flashlightLight.enabled = true;
        }

        // --- NEW QUEST LOG INTEGRATION ---
        if (questManager != null)
        {
            // Call the manager to update the progress for this objective
            questManager.UpdateObjectiveProgress(objectiveDescription);
        }
        // ---------------------------------

        Debug.Log("Flashlight picked up and equipped!");
    }

    private void ToggleFlashlight()
    {
        if (flashlightLight == null) return;

        // Toggle the state
        _isFlashlightOn = !_isFlashlightOn;
        flashlightLight.enabled = _isFlashlightOn;

        Debug.Log($"Flashlight is now: {(_isFlashlightOn ? "ON" : "OFF")}");
    }

    public bool HasFlashlight()
    {
        return _hasFlashlight;
    }
}