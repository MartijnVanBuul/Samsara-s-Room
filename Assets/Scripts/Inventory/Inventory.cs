using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory instance;

    public List<PickUpInteractable> interactables;

    public delegate void InteractableAddedEvent(PickUpInteractable addedInteractable);
    public InteractableAddedEvent OnInteractableAdded;

    public delegate void InteractableRemovedEvent(PickUpInteractable removedInteractable);
    public InteractableRemovedEvent OnInteractableRemoved;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void AddInteractable(PickUpInteractable interactable)
    {
        if (!interactables.Contains(interactable))
        {
            interactables.Add(interactable);
            if (OnInteractableAdded != null)
                OnInteractableAdded(interactable);
        }
    }

    public void RemoveInteractable(PickUpInteractable interactable)
    {
        if (interactables.Contains(interactable) && interactable.consume)
        {
            interactables.Remove(interactable);
            if (OnInteractableRemoved != null)
                OnInteractableRemoved(interactable);
        }
    }
}
