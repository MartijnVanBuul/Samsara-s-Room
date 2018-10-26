using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour {

    public static InteractablesManager instance;

    public List<Interactable> interactables;

    private void Awake()
    {
        if(!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void AddInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
    }

    public void RemoveInteractable(Interactable interactable)
    {
        interactables.Remove(interactable);
    }
}
