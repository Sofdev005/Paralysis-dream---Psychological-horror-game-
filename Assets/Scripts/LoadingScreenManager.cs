using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Use if you are using TextMeshPro for the Text component

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Scene Loading")]
    [Tooltip("The name or index of the scene to load (e.g., 'Level_1').")]
    public string sceneToLoad = "Level_1";
    // Alternatively, you could use a public int sceneIndexToLoad = 1;

    [Header("UI References")]
    [Tooltip("The parent panel that holds all loading UI elements.")]
    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private Slider progressBar;
    // If using TextMeshPro, change Text to TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI loadingText;

    // Static Instance to easily call the loading function from other scripts
    public static LoadingScreenManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the manager alive during scene transition
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Public function to start the loading process, called by the Main Menu button.
    /// </summary>
    public void LoadNewScene()
    {
        // 1. Show the loading screen UI
        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(true);
        }

        // 2. Start the asynchronous loading routine
        StartCoroutine(LoadAsynchronously(sceneToLoad));
    }

    private IEnumerator LoadAsynchronously(string sceneName)
    {
        // Start the background loading operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevent the new scene from immediately activating once it's finished loading
        // (This allows you to hold the screen on 100% until a button is pressed if you want)
        // operation.allowSceneActivation = false; 

        while (!operation.isDone)
        {
            // operation.progress goes from 0.0 to 0.9. We remap it to 0.0 to 1.0 for a cleaner bar.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update the UI
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (loadingText != null)
            {
                // Display the percentage (e.g., 55%)
                loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
            }

            // Wait until the next frame before checking again
            yield return null;
        }

        // Optional: If you used 'operation.allowSceneActivation = false;', you can set it to true here
        // or wait for player input before setting it to true.

        // Loading is 100% complete, but the loop finishes only when the scene is active.
        Debug.Log($"Scene '{sceneName}' loaded successfully.");
    }
}