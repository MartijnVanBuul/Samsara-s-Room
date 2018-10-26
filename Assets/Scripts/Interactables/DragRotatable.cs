using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotatable : DragInteractable
{

    public delegate void RotationUpdate(Vector3 rotation);
    public RotationUpdate onRotationUpdate;

    public event RotationUpdate onRotationChanged;

    [SerializeField]
    private bool isSmooth;
    [SerializeField]
    private float smoothSpeed = 8;
    [SerializeField]
    private bool useInitialRotation;
    [SerializeField]
    private bool isLocalRotation;
    [SerializeField]
    private Transform rotatable;
    [SerializeField]
    private Vector3 rotationAxis = Vector3.up;

    private float targetRotation;
    private float initialRotation;
    private Vector3 initialEuler;
    private Vector3 currentRotationAxis;
    private float currentRotation;

    private void Start()
    {
        if (!rotatable)
            rotatable = transform;


    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (isBeingDragged)
        {
            if (isSmooth)
                targetRotation = Mathf.MoveTowardsAngle(currentRotation,  -Mathf.Sign((currentPositionWorld - (Vector2)rotatable.transform.position).x) * Vector3.Angle(Vector3.up, (currentPositionWorld - (Vector2)rotatable.transform.position)), smoothSpeed);
            else
                targetRotation = -Mathf.Sign((currentPositionWorld - (Vector2)rotatable.transform.position).x) * Vector3.Angle(Vector3.up, (currentPositionWorld - (Vector2)rotatable.transform.position));
            
            rotatable.Rotate(currentRotationAxis, targetRotation - currentRotation);
            currentRotation = targetRotation;

            if (onRotationChanged != null)
                onRotationChanged(transform.eulerAngles);
        }
    }

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        if (useInitialRotation)
        {
            initialRotation = currentRotation = -Mathf.Sign((startPositionWorld - (Vector2)rotatable.transform.position).x) * Vector3.Angle(Vector3.up, (startPositionWorld - (Vector2)rotatable.transform.position));
            initialEuler = rotatable.transform.eulerAngles;

            if (!isLocalRotation)
                currentRotationAxis = Vector3.Normalize(rotatable.transform.InverseTransformDirection(rotationAxis));
            else
                currentRotationAxis = rotationAxis;
        }
    }

    protected override void DragReleased(Vector2 position)
    {
        base.DragReleased(position);

        if (onRotationUpdate != null)
            onRotationUpdate(rotatable.transform.eulerAngles);
    }

    public Vector3 GetCurrentRotation()
    {
        return rotatable.transform.eulerAngles;
    }
}
