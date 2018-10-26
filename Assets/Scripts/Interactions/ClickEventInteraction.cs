using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickEventInteraction : Interactable {

    [SerializeField]
    private UnityEvent clickEvent;

    public override void Interact(Vector2 position)
    {
        base.Interact(position);
        clickEvent.Invoke();
    }
}
