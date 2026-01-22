using UnityEngine;

/// <summary>
/// Plays background music when the player enters the attached Box Collider (must be a Trigger).
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlayAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("The AudioSource component containing the music clip to play.")]
    [SerializeField] private AudioSource backgroundMusicSource;

    [Tooltip("If checked, the music will stop when the player exits the trigger.")]
    [SerializeField] private bool stopOnExit = false;

    private Collider _myCollider;

    void Awake()
    {
        _myCollider = GetComponent<Collider>();

        // IMPORTANT: Ensure the Collider is set to be a Trigger
        if (!_myCollider.isTrigger)
        {
            Debug.LogError($"Collider on '{gameObject.name}' is not set to 'Is Trigger'. MusicTrigger will not work.", this);
        }

        // Ensure the AudioSource is assigned
        if (backgroundMusicSource == null)
        {
            Debug.LogError($"AudioSource is not assigned on '{gameObject.name}'. Music will not play.", this);
        }
    }

    /// <summary>
    /// Called when another collider enters this trigger.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the Player
        // You might use a specific tag like "Player" or check for a Player component.
        if (other.CompareTag("Player"))
        {
            if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
                Debug.Log($"Music started playing in area: {gameObject.name}");
            }
        }
    }

    /// <summary>
    /// Called when another collider exits this trigger.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (stopOnExit && other.CompareTag("Player"))
        {
            if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Stop();
                Debug.Log($"Music stopped playing in area: {gameObject.name}");
            }
        }
    }
}