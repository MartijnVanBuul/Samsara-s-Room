using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInteraction : Interaction {

    [Header("Rotate properties")]
    public Vector3 rotationAmount;

    public delegate void RotationChangedEvent();
    public RotationChangedEvent onRotationChanged;

    public override void PerformInteraction()
    {
        base.PerformInteraction();

        transform.Rotate(rotationAmount);

        if (onRotationChanged != null)
            onRotationChanged();
    }
}
