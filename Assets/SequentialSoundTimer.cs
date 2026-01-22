using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SequentialSoundTimer : MonoBehaviour
{
    // --- Configuration Variables ---
    [Header("Timing")]
    [Tooltip("Time delay between playing each sound (in seconds)")]
    public float timeBetweenCues = 10f;

    [Header("Audio Setup")]
    public AudioSource audioPlayer;
    public AudioClip[] soundSequence;

    // --- Private Tracking Variables ---
    private int currentClipIndex = 0;

    // --- Initialization ---
    void Start()
    {
        // ... (Initial checks remain unchanged) ...
        if (audioPlayer == null || soundSequence == null || soundSequence.Length == 0)
        {
            // Simplified error check for brevity
            Debug.LogError("SequentialSoundTimer setup failed. Check AudioSource or Sound Sequence array.");
            enabled = false;
            return;
        }

        StartCoroutine(SequenceCycleCoroutine());
    }

    // --- Main Repeating Timer and Player ---
    IEnumerator SequenceCycleCoroutine()
    {
        // *************************************************************
        // *** NEW: WAIT BEFORE THE VERY FIRST SOUND PLAYS ***
        // *************************************************************
        Debug.Log("Waiting for initial timer: " + timeBetweenCues + " seconds...");
        yield return new WaitForSeconds(timeBetweenCues);

        while (true) // Infinite loop to keep the sequence running
        {
            // 1. Play the current sound in the sequence
            PlayNextCue();

            // 2. Wait for the designated time (e.g., 10 seconds)
            // This also handles the wait between subsequent cues.
            yield return new WaitForSeconds(timeBetweenCues);
        }
    }

    // --- Logic to select and play the next clip (Unchanged) ---
    void PlayNextCue()
    {
        if (soundSequence.Length == 0) return;

        audioPlayer.clip = soundSequence[currentClipIndex];
        audioPlayer.Play();

        Debug.Log("Playing sound #" + currentClipIndex + ": " + audioPlayer.clip.name);

        currentClipIndex++;

        if (currentClipIndex >= soundSequence.Length)
        {
            currentClipIndex = 0;
            Debug.Log("Sound sequence finished, resetting loop.");
        }
    }
}