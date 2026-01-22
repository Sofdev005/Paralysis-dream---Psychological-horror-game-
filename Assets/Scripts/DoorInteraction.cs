using UnityEngine;
using DG.Tweening; // IMPORTANT: Requires the DOTween asset to be installed

/// <summary>
/// Example implementation of IInteractable for a door that toggles between open and closed state.
/// Attach this script to the door GameObject.
/// </summary>
public class DoorInteraction : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [Tooltip("The rotation to apply when opening. E.g., (0, 90, 0) for a standard swing.")]
    [SerializeField] private Vector3 _openRotation = new Vector3(0, 90f, 0f);
    [SerializeField] private float _rotationDuration = 0.5f;

    [Header("State")]
    // Set this to TRUE in the Inspector for the final locked door
    [SerializeField] private bool _isLocked = true; // Defaulting to true for the quest door
    private bool _isOpen = false;

    // --- IInteractable Implementation ---

    public bool CanInteract()
    {
        // Player can always interact, but the result depends on _isLocked state
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        if (_isLocked)
        {
            Debug.Log("The door is locked. Find the key to unlock it first.");
            // Do not proceed with opening logic if locked
            return false;
        }

        // Toggle the open state
        _isOpen = !_isOpen;

        Vector3 targetRotation;

        if (_isOpen)
        {
            // Opening: Apply the full open rotation
            targetRotation = _openRotation;
        }
        else
        {
            // Closing: Rotate back to the identity (0, 0, 0) rotation
            targetRotation = Vector3.zero;
        }

        // Use DOTween to smoothly rotate the door
        transform.DOLocalRotate(targetRotation, _rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine);

        return true;
    }

    public string GetDescription()
    {
        if (_isLocked)
        {
            return "Locked Door";
        }

        return _isOpen ? "Close Door (E)" : "Open Door (E)";
    }

    /// <summary>
    /// PUBLIC METHOD: Called by the KeyItem to unlock this door.
    /// </summary>
    public void UnlockDoor()
    {
        _isLocked = false;
        Debug.Log(gameObject.name + " has been unlocked! Press 'E' to open it.");
    }
}