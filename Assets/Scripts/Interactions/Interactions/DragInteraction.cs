using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInteraction : DragInteractable {

    public override void Update()
    {
        base.Update();


        if (isBeingDragged && currentPositionWorld != Vector2.zero)
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().MovePosition(new Vector3(currentPositionWorld.x, currentPositionWorld.y, transform.position.z));
            else
                transform.position = new Vector3(currentPositionWorld.x, currentPositionWorld.y, transform.position.z);
        }
    }
}
