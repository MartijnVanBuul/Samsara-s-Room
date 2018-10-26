using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private void Start()
    {
        if (InteractablesManager.instance)
            InteractablesManager.instance.AddInteractable(this);
    }

    public virtual void Interact(Vector2 position)
    {
        Debug.Log("Interaction with " + name);

        //We also invalidate the current swipe interaction in case we are swiping.
        if (SwipeDetection.instance)
        {
            SwipeDetection.instance.InvalidateCurrentSwipeMotion();
        }
    }
}
