using UnityEngine;

/// <summary>
/// An IInteractable object that adds itself to the inventory and destroys itself.
/// </summary>
public class CollectableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Mystery Item";
    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        // 1. Update the central inventory system
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddCollectedItem();
        }

        // 2. Hide or destroy the item model
        // Destroy(gameObject); // Use this if you want it permanently gone
        gameObject.SetActive(false); // Use this if you want to reuse the object or pool it

        return true;
    }

    public string GetDescription()
    {
        return $"Collect {itemName} (E)";
    }
}