using UnityEngine;

public class Billboarder : MonoBehaviour
{
    // A public variable to hold the reference to the main camera's transform.
    // If you leave this unassigned in the Inspector, the script will try to find the Main Camera automatically.
    [SerializeField]public Transform cameraTransform;

    [Tooltip("If true, the sprite will only rotate around the Y-axis (useful for standing characters).")]
    [SerializeField]public bool lockXandZAxis = true;

    void Start()
    {
        // If the camera transform hasn't been assigned manually, find the Main Camera.
        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogError("Billboarder failed: No 'Main Camera' tag found! Please assign the camera manually.");
                enabled = false; // Disable the script if no camera is found
            }
        }
    }

    // Update is called every frame and is necessary for constant rotation
    void Update()
    {
        if (cameraTransform == null) return;

        if (lockXandZAxis)
        {
            // --- AXIAL BILLBOARDING (Best for standing characters/trees) ---
            // 1. Get the camera's position.
            Vector3 targetPosition = cameraTransform.position;

            // 2. Lock the Y-axis of the target position to the sprite's Y-axis.
            // This prevents the sprite from tilting up or down, keeping it standing straight.
            targetPosition.y = transform.position.y;

            // 3. Make the sprite look at the adjusted position.
            transform.LookAt(targetPosition);
        }
        else
        {
            // --- FULL BILLBOARDING (Best for particle effects, clouds, etc.) ---
            // The sprite rotates completely to face the camera, tilting on all axes.
            transform.LookAt(cameraTransform.position);
        }
    }
}