using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interaction))]
[RequireComponent(typeof(Collider))]
public class PerformInteractable : Interactable
{
    public event Action OnPerformInteraction;

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        PerformInteraction();
    }

    public void PerformInteraction()
    {
        if (GetComponent<Interaction>())
            GetComponent<Interaction>().PerformInteraction();

        if (OnPerformInteraction != null)
        {
            OnPerformInteraction();
        }
    }
}
