using UnityEngine;

/// <summary>
/// An IInteractable object that unlocks a specific DoorInteraction script.
/// </summary>
public class KeyItem : MonoBehaviour, IInteractable
{
    [Tooltip("Drag the specific Door GameObject this key is meant to unlock.")]
    [SerializeField] private DoorInteraction targetDoor;

    // --- IInteractable Implementation ---

    public bool CanInteract()
    {
        return targetDoor != null;
    }

    public bool Interact(Interactor interactor)
    {
        if (targetDoor == null)
        {
            Debug.LogError("KeyItem has no target door assigned!", this);
            return false;
        }

        // 1. Call the public UnlockDoor method on the target door
        targetDoor.UnlockDoor();

        // 2. Give feedback (Optional: play sound, visual effect)

        // 3. Destroy the key after use
        Destroy(gameObject);

        return true;
    }

    public string GetDescription()
    {
        return "Use Key on Door (E)";
    }
}