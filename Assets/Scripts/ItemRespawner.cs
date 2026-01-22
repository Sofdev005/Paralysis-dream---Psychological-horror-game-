using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    public Transform[] spawnPoints;   // Assign 3 spawn points for this object
    public float respawnTime = 5f;

    private int currentIndex = 0;
    private float timer;
    private bool stopRespawning = false;
    [SerializeField] private AudioSource soundA;

    void Update()
    {
        // If stopped, do nothing
        if (stopRespawning)
            return;

        // Handle respawn timer
        timer += Time.deltaTime;

        if (timer >= respawnTime)
        {
            timer = 0f;
            currentIndex = (currentIndex + 1) % spawnPoints.Length;
            transform.position = spawnPoints[currentIndex].position;
            gameObject.SetActive(true);
            soundA.Play();
        }

        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))// Stop respawning when pressing E and in collision with the obj
        {
            stopRespawning = true;
        }
    }

}
