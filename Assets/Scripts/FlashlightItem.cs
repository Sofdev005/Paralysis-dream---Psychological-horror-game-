using UnityEngine;

/// <summary>
/// Represents the physical flashlight object in the world that can be picked up.
/// </summary>
public class FlashlightItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string pickupDescription = "Press E to Pick Up Flashlight";
    private bool _isPickedUp = false;

    // IInteractable Implementation
    public string GetDescription()
    {
        return pickupDescription;
    }

    public bool CanInteract()
    {
        // The flashlight can be interacted with only if it hasn't been picked up yet
        return !_isPickedUp;
    }

    public bool Interact(Interactor interactor)
    {
        if (_isPickedUp)
        {
            return false; // Already picked up
        }

        // 1. Get the player's FlashlightController component
        FlashlightController controller = interactor.GetComponent<FlashlightController>();

        if (controller == null)
        {
            // Fallback for setup where the Interactor is on the Player and the Controller is on a Child/Parent
            controller = interactor.GetComponentInParent<FlashlightController>();
            if (controller == null)
            {
                Debug.LogError("FlashlightController not found on the Interactor or its parent/children. Cannot pick up flashlight.");
                return false;
            }
        }

        // 2. Tell the controller that the item has been picked up
        controller.PickupFlashlight();

        // 3. Mark the item as picked up and destroy the physical object in the world
        _isPickedUp = true;

        // OPTIONAL: Play a sound effect here!

        // Remove the object from the scene
        Destroy(gameObject);

        // You may want to also hide the interaction UI manually here if it persists for a frame.
        // The Interactor script's next Update loop should handle the hiding.

        return true;
    }
}   