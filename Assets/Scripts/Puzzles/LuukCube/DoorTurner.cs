using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTurner : Interactable {

    [SerializeField]
    private List<Transform> doors;

    public void TurnDoors()
    {
        foreach (Transform door in doors)
            door.Rotate(Vector3.forward * 90);
    }

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        TurnDoors();
    }
}
