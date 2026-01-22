using UnityEngine;
using DG.Tweening;

/// <summary>
/// Example implementation of IInteractable for a drawer that slides open and closed.
/// Attach this script to the drawer GameObject (ensuring its Z-axis is the sliding direction).
/// </summary>
public class DrawerInteraction : MonoBehaviour, IInteractable
{
    [Header("Movement Settings")]
    [Tooltip("How far the drawer slides out along its local Z-axis (or X, depending on model).")]
    [SerializeField] private float _openDistance = 0.4f;
    [SerializeField] private float _moveDuration = 0.4f;
    [SerializeField] private bool _isSlidingOnXAxis = false; // Toggle for X-axis movement

    private Vector3 _closedPosition;
    private bool _isOpen = false;

    // RENAMED for better semantic meaning (plays on both open and close)
    [SerializeField] public AudioSource moveSound;

    void Start()
    {
        // Store the initial position of the drawer
        _closedPosition = transform.localPosition;
    }

    // --- IInteractable Implementation ---

    public bool CanInteract()
    {
        // Assume drawers are always interactable unless you add a lock flag
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        _isOpen = !_isOpen;

        Vector3 targetPosition;
        if (_isOpen)
        {
            // Calculate the open position relative to the closed position
            if (_isSlidingOnXAxis)
            {
                targetPosition = _closedPosition + transform.right * _openDistance;
            }
            else
            {
                // Default to local Z-axis (forward)
                targetPosition = _closedPosition + transform.forward * _openDistance;
            }
        }
        else
        {
            // Closing: Return to the initial closed position
            targetPosition = _closedPosition;
        }

        // Use DOTween to smoothly move the drawer
        transform.DOLocalMove(targetPosition, _moveDuration)
            .SetEase(Ease.OutQuad);

        // PLAYS THE AUDIO ON BOTH OPEN AND CLOSE INTERACTION
        moveSound.Play();

        return true;
    }

    public string GetDescription()
    {
        return _isOpen ? "Close Drawer (E)" : "Open Drawer (E)";
    }
}