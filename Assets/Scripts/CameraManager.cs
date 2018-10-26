using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_CameraMode
{
    CameraAroundPivot,
    CameraRotateSelf
}

public class CameraManager : MonoBehaviour {

    public static CameraManager instance;

    private E_CameraMode currentCameraMode;
    public CameraNode currentNode;

    private Vector3 currentRotation;
    private float rotateSpeed = 8;
    private float moveSpeed = 8;

    private bool isTransitioning;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (currentNode)
            SetCameraNode(currentNode);
    }

    private void Update()
    {

        if (currentNode)
        {
            if (isTransitioning)
                transform.position = Vector3.MoveTowards(transform.position, Quaternion.Euler((currentNode.MinimumRotation + currentNode.MaximumRotation) / 2) * currentNode.transform.forward * currentNode.Offset, Time.deltaTime * moveSpeed * Mathf.Clamp(Mathf.Sqrt(Vector3.Distance(transform.position, currentNode.transform.forward * currentNode.Offset)), 0.05f, 10));

            transform.LookAt(currentNode.transform);
        }

        if(isTransitioning && Vector3.Distance(transform.position, Quaternion.Euler((currentNode.MinimumRotation + currentNode.MaximumRotation) / 2) * currentNode.transform.forward * currentNode.Offset) < 0.05f)
        {
            transform.position = Quaternion.Euler((currentNode.MinimumRotation + currentNode.MaximumRotation) /2) * currentNode.transform.forward * currentNode.Offset;
            transform.SetParent(currentNode.transform.GetChild(0));
            isTransitioning = false;
        }
    }

    public void ChangeCameraMode(E_CameraMode cameraMode)
    {
        currentCameraMode = cameraMode;
    }

    public void SetCameraNode(CameraNode cameraNode)
    {
        if (currentNode)
            cameraNode.SetSelected(false);

        isTransitioning = true;
        cameraNode.SetSelected(true);
        currentCameraMode = cameraNode.MyCameraMode;
        currentNode = cameraNode;
        transform.LookAt(cameraNode.transform);
        cameraNode.transform.GetChild(0).eulerAngles = (cameraNode.MinimumRotation + cameraNode.MaximumRotation) / 2f;
    }

    public void MoveToPreviousNode()
    {
        if (currentNode.PreviousNode)
            SetCameraNode(currentNode.PreviousNode);
    }

    public void MoveToNextNode()
    {
        if (currentNode.NextNode)
            SetCameraNode(currentNode.NextNode);
    }
}
