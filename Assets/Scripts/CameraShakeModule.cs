using UnityEngine;
using System.Collections;

public class CameraShakeModule : MonoBehaviour
{
    private Vector3 originalLocalPos;

    void Awake()
    {
        // Store the camera's starting local position (relative to its parent, e.g., the player body/head)
        originalLocalPos = transform.localPosition;
    }

    /// <summary>
    /// Starts the camera shaking effect for a specified duration and magnitude.
    /// </summary>
    public void StartShake(float duration, float magnitude)
    {
        // Stop any currently running shake coroutines to prevent stacking
        StopAllCoroutines();

        // Start the new shake coroutine
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generate random offsets for x and y, scaled by the magnitude
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the offset in local space relative to the original position
            transform.localPosition = originalLocalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // When the duration is over, smoothly return the camera to its resting position
        transform.localPosition = originalLocalPos;
    }
}