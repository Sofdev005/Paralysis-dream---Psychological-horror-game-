using UnityEngine;

public interface IInteractable
{
    bool Interact(Interactor interactor);
    bool CanInteract();

    string GetDescription();
}