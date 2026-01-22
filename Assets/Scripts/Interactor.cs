using UnityEngine;
using TMPro; // Assuming you use TextMeshPro for UI

/// <summary>
/// Handles raycasting, UI feedback, and input for interacting with IInteractable objects.
/// Attach this script to your Player object.
/// </summary>
public class Interactor : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera mainCam;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("UI Feedback")]
    // The GameObject that contains the interaction prompt text (e.g., a background panel)
    [SerializeField] private GameObject interactionUI;
    // The Text component to display the object description (e.g., "Press E to Open")
    [SerializeField] private TextMeshProUGUI interactionText;

    private RaycastHit _hit;
    private IInteractable _currentInteractable;

    void Start()
    {
        // Ensure UI elements are hidden at start
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    void Update()
    {
        // Perform the raycast every frame to check for objects
        DoInteractionRaycast();

        // Handle input if an interactable object is found
        if (_currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            // Check if interaction is currently allowed before calling Interact()
            if (_currentInteractable.CanInteract())
            {
                // Pass 'this' (the Interactor instance) to the Interact method
                _currentInteractable.Interact(this);
            }
        }
    }

    private void DoInteractionRaycast()
    {
        // Ray starts from the center of the camera viewport and goes forward
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
        bool hitSomething = false;

        // Reset the current interactable object
        _currentInteractable = null;

        if (Physics.Raycast(ray, out _hit, interactionDistance))
        {
            // Try to get the IInteractable component from the object hit
            IInteractable interactable = _hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                _currentInteractable = interactable;
                hitSomething = true;

                // Update UI text only if interaction is allowed
                if (interactable.CanInteract())
                {
                    interactionText.text = interactable.GetDescription();
                }
                else
                {
                    // Hide UI if it's interactable but not ready (e.g., locked)
                    hitSomething = false;
                }
            }
        }

        // Show/hide the UI based on whether a valid, ready interactable was found
        if (interactionUI != null)
        {
            interactionUI.SetActive(hitSomething);
        }
    }
}