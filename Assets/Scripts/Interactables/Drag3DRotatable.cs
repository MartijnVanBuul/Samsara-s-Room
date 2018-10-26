using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag3DRotatable : DragInteractable {

    public delegate void RotationUpdate(Vector3 rotation);
    public RotationUpdate onRotationUpdate;

    [SerializeField]
    private float rotationRatio = 0.1f;

    // Update is called once per frame
    public override void Update () {
        base.Update();

        if (isBeingDragged)
            transform.Rotate(new Vector3((currentPositionWorld - (Vector2)transform.position).y, -(currentPositionWorld - (Vector2)transform.position).x, 0) * rotationRatio, Space.World); 
    }

    protected override void DragReleased(Vector2 position)
    {
        base.DragReleased(position);
        
            if (onRotationUpdate != null)
                onRotationUpdate(transform.eulerAngles);
    }

    public Vector3 GetCurrentRotation()
    {
        return transform.eulerAngles;
    }
}
