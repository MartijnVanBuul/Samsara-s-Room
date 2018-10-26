using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNode : MonoBehaviour {
    [Header("Node information")]
    public E_CameraMode MyCameraMode;
    public float Offset;
    public Vector2 MinimumRotation;
    public Vector2 MaximumRotation;
    public Vector2 InitialRotation;
    public bool IsSelected;
    public bool IsInteractable;

    [Header("Camera nodes")]
    public CameraNode NextNode;
    public CameraNode PreviousNode;

    private Vector2 initialRotation;
    private Vector3 currentRotation;
    private bool isDragging;

    private Vector2 currentDragSpeed;
    private float rotationSpeed = 0.125f;


    private void Start()
    {
        currentRotation = transform.GetChild(0).localEulerAngles;
    }

    private void Update()
    {

        if (isDragging)
        {
            currentDragSpeed *= 0.5f;
            currentDragSpeed += InputProcessor.instance.GetDeltaPosition() / 50;

            if (Mathf.RoundToInt(MaximumRotation.x) - Mathf.RoundToInt(MinimumRotation.x) == 360)
                currentRotation.z = currentRotation.z - InputProcessor.instance.GetDeltaPosition().y * rotationSpeed;
            else
                currentRotation.z = Mathf.Clamp(currentRotation.z - InputProcessor.instance.GetDeltaPosition().y * rotationSpeed, MinimumRotation.x, MaximumRotation.x);

            if (Mathf.RoundToInt(MaximumRotation.y) - Mathf.RoundToInt(MinimumRotation.y) == 360)
                currentRotation.y = currentRotation.y - InputProcessor.instance.GetDeltaPosition().x * rotationSpeed;
            else
                currentRotation.y = Mathf.Clamp(currentRotation.y - InputProcessor.instance.GetDeltaPosition().x * rotationSpeed, MinimumRotation.y, MaximumRotation.y);
        }
        else
        {
            currentDragSpeed *= 0.9f;

            if (Mathf.RoundToInt(MaximumRotation.x) - Mathf.RoundToInt(MinimumRotation.x) == 360)
                currentRotation.z = currentRotation.z - currentDragSpeed.y;
            else
                currentRotation.z = Mathf.Clamp(currentRotation.z - currentDragSpeed.y, MinimumRotation.x, MaximumRotation.x);

            if (Mathf.RoundToInt(MaximumRotation.y) - Mathf.RoundToInt(MinimumRotation.y) == 360)
                currentRotation.y = currentRotation.y - currentDragSpeed.x;
            else
                currentRotation.y = Mathf.Clamp(currentRotation.y - currentDragSpeed.x, MinimumRotation.y, MaximumRotation.y);
        }

        transform.GetChild(0).localEulerAngles = currentRotation;
    }

    public void SelectNode()
    {
        if(CameraManager.instance && CameraManager.instance.currentNode != this)
            CameraManager.instance.SetCameraNode(this);

        currentRotation = InitialRotation;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.75F);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(MinimumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(MinimumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(MaximumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(MaximumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.color = new Color(1, 0.2f, 0.2f, 0.75F);
        Gizmos.DrawLine(transform.position + Quaternion.Euler(MaximumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset, transform.position + Quaternion.Euler(MinimumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position + Quaternion.Euler(MinimumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset, transform.position + Quaternion.Euler(MinimumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position + Quaternion.Euler(MinimumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset, transform.position + Quaternion.Euler(MaximumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.DrawLine(transform.position + Quaternion.Euler(MaximumRotation.x, MaximumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset, transform.position + Quaternion.Euler(MaximumRotation.x, MinimumRotation.y, 0) * Quaternion.AngleAxis(0, Vector3.up) * Vector3.forward * Offset);
        Gizmos.color = new Color(0, 1, 0, 0.75F);

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)InitialRotation * Offset);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (NextNode)
            Gizmos.DrawLine(transform.position, NextNode.transform.position);

        if (PreviousNode)
            Gizmos.DrawLine(transform.position, PreviousNode.transform.position);
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;

        if (IsSelected && InputProcessor.instance && InteractionManager.instance)
        {
            InteractionManager.instance.onActionDetermined += ActionDetermined;
            InputProcessor.instance.onClickRelease += OnClickRelease;

        }
        else
        {
            InteractionManager.instance.onActionDetermined -= ActionDetermined;
            InputProcessor.instance.onClickRelease -= OnClickRelease;
        }
    }


    private void OnClickRelease(Vector2 clickPosition)
    {
        isDragging = false;
    }

    private void ActionDetermined(E_Action action, Interactable interactable)
    {
        Debug.Log(action.ToString());
        if(action == E_Action.Rotate)
        {
            currentDragSpeed = Vector2.zero;
            initialRotation = transform.GetChild(0).localEulerAngles;
            isDragging = true; ;
        }
        else
        {
            isDragging = false;
        }
    }
}
