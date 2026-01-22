using UnityEngine;
using UnityEngine.Rendering; // Primary URP/HDRP namespace for Volumes
using UnityEngine.Rendering.Universal; // URP-specific effects namespace
using System.Collections;

public class RecurringBlurEffect : MonoBehaviour
{
    // --- Configuration Variables ---
    [Header("Timing")]
    [Tooltip("Time between blur events (in seconds)")]
    public float timeBetweenBlurs = 45f;
    [Tooltip("Total duration of the blur/disorientation effect (in seconds)")]
    public float blurDuration = 5f;
    [Tooltip("Time it takes to fade the blur in and out")]
    public float transitionTime = 1.0f;

    [Header("Post Processing")]
    [Tooltip("The URP Volume component in the scene")]
    // NOTE: This uses the URP 'Volume' component
    public Volume volume;

    [Header("Camera Shake")]
    [Tooltip("Reference to the CameraShakeModule on the Main Camera")]
    public CameraShakeModule cameraShaker;
    public float shakeMagnitude = 0.05f;
    [Header("audio")]
    [SerializeField] public AudioSource voices;

    // The URP-specific effect we want to control
    private DepthOfField depthOfField;

    // Constants for the blur intensity (you may need to adjust these URP values)
    private const float NormalFocusDistance = 10f; // High value = clear focus
    private const float BlurredFocusDistance = 0.1f; // Low value = blurred focus

    void Start()
    {
        // 1. Get the reference to the URP Depth of Field settings from the current profile
        // The URP equivalent function is 'TryGet'
        if (volume.profile.TryGet<DepthOfField>(out depthOfField))
        {
            // Ensure blur is off at the start
            // NOTE: URP uses 'distance' for focus, not 'aperture' for this effect
            depthOfField.focusDistance.value = NormalFocusDistance;

            // Start the main repeating coroutine
            StartCoroutine(BlurCycleCoroutine());
        }
        else
        {
            Debug.LogError("Depth Of Field effect not found on the URP Volume profile. Did you add the override?");
        }
    }

    // --- Main Repeating Timer ---
    IEnumerator BlurCycleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenBlurs);
            StartCoroutine(BlurSequence());
        }
    }

    // --- Combined Blur and Shake Sequence ---
    IEnumerator BlurSequence()
    {
        // 1. Trigger the Camera Shake
        if (cameraShaker != null)
        {
            cameraShaker.StartShake(blurDuration, shakeMagnitude);
            voices.Play();
        }

        // 2. Fade IN (Transition to Blur)
        float timer = 0f;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = timer / transitionTime;

            // Lerp the Focus Distance from Normal to Blurred
            depthOfField.focusDistance.value = Mathf.Lerp(NormalFocusDistance, BlurredFocusDistance, t);
            yield return null;
        }

        // 3. Hold Blur (Peak Disorientation)
        yield return new WaitForSeconds(blurDuration - (transitionTime * 2));

        // 4. Fade OUT (Transition to Normal)
        timer = 0f;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = timer / transitionTime;

            // Lerp the Focus Distance back to Normal
            depthOfField.focusDistance.value = Mathf.Lerp(BlurredFocusDistance, NormalFocusDistance, t);
            yield return null;
        }
    }
}