using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickUpInteractable : Interactable
{

    [Header("Pick up properties")]
    public string Name;
    public Sprite PickUpSprite;

    public bool consume = true;
    public bool HasBeenTaken = false;

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        PickUp();
    }

    public void PickUp()
    {
        Inventory.instance.AddInteractable(this);
        InteractablesManager.instance.RemoveInteractable(this);

        HasBeenTaken = true;
        gameObject.SetActive(false);
    }
}
