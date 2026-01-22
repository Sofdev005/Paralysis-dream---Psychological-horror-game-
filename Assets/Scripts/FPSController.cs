using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    // --- Existing Variables ---
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Camera playerCam;
    private float xRotation = 0f;

    // --- NEW AUDIO VARIABLES ---
    [Header("Audio")]
    [Tooltip("The AudioSource component used to play the walking sound.")]
    public AudioSource footstepsSource;

    [Tooltip("The actual AudioClip for the walking sound.")]
    public AudioClip walkSoundClip;

    private bool isMoving = false; // Flag to track movement state

    // --- Start ---
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;

        // NEW: Check if the required components are set up
        if (footstepsSource == null || walkSoundClip == null)
        {
            Debug.LogWarning("Footsteps audio setup is incomplete. Walking sounds disabled.");
        }
        else
        {
            // NEW: Assign the clip and ensure it's ready to loop
            footstepsSource.clip = walkSoundClip;
            footstepsSource.loop = true; // IMPORTANT: Set the audio source to loop!
            footstepsSource.playOnAwake = false;
        }
    }

    // --- Update ---
    void Update()
    {
        // Mouse Look (unchanged)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * walkSpeed * Time.deltaTime);
        controller.Move(Vector3.down * 9.81f * Time.deltaTime); // Gravity

        // --- NEW AUDIO LOGIC ---

        // Check if there is any movement input (either X or Z is non-zero)
        bool inputDetected = (x != 0 || z != 0);

        // Check if the character is grounded AND movement input is detected
        bool shouldBeMoving = controller.isGrounded && inputDetected;

        if (footstepsSource != null)
        {
            if (shouldBeMoving && !isMoving)
            {
                // Start playing sound if we just started moving
                footstepsSource.Play();
                isMoving = true;
            }
            else if (!shouldBeMoving && isMoving)
            {
                // Stop sound if we just stopped moving
                footstepsSource.Stop();
                isMoving = false;
            }
            // Note: If shouldBeMoving and isMoving are both true, the sound keeps playing (looping).
        }
        // --- END AUDIO LOGIC ---
    }
}