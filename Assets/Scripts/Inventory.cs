using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Key / Collectible settings")]
    [Tooltip("How many collectibles required to get the key.")]
    [SerializeField] private int requiredToGetKey = 3;

    // public so other systems (Door) can read it
    public bool haskey { get; private set; } = false;

    // internal count
    private int collectedCount = 0;

    public void CollectItem(GameObject item)
    {
        if (item == null) return;

        // Make the object disappear
        item.SetActive(false);

        // increment and clamp
        collectedCount = Mathf.Min(requiredToGetKey, collectedCount + 1);
        Debug.Log($"Collected item. Count = {collectedCount}/{requiredToGetKey}");

        // grant key if we've reached the target
        if (!haskey && collectedCount >= requiredToGetKey)
        {
            haskey = true;
            Debug.Log("Key acquired! You can now open the door.");
            // Optional: raise an event here for UI or sound
        }
    }

    public void ResetProgress()
    {
        collectedCount = 0;
        haskey = false;
    }

}
