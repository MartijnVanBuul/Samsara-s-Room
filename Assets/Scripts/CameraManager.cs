using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private float touchDistance;
    private float pinchRatio = 1.2f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (currentNode)
            SetCameraNode(currentNode);

        if(InteractionManager.instance)
            InteractionManager.instance.onActionDetermined += ActionDetermined;

    }

    private void Update()
    {

        if (currentNode)
        {
            if (isTransitioning)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position + Quaternion.Euler(currentNode.InitialRotation) * currentNode.transform.right * currentNode.Offset, Time.deltaTime * moveSpeed * Mathf.Clamp(Mathf.Sqrt(Vector3.Distance(transform.position, currentNode.transform.forward * currentNode.Offset)), 0.05f, 10));
                transform.LookAt(currentNode.transform.GetChild(0));
            }
        }

        if(isTransitioning && Vector3.Distance(transform.position, currentNode.transform.position + Quaternion.Euler(currentNode.InitialRotation) * currentNode.transform.right * currentNode.Offset) < 0.05f)
        {
            transform.position = currentNode.transform.position + Quaternion.Euler(currentNode.InitialRotation) * currentNode.transform.right * currentNode.Offset;
            transform.SetParent(currentNode.transform.GetChild(0));
            transform.LookAt(currentNode.transform.GetChild(0));
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
            currentNode.SetSelected(false);

        if (cameraNode.GetComponent<BoxCollider>())
            cameraNode.GetComponent<BoxCollider>().enabled = false;

        if (currentNode.GetComponent<BoxCollider>())
            currentNode.GetComponent<BoxCollider>().enabled = true;

        if (InteractionManager.instance)
        {
            if (cameraNode.IsInteractable)
                InteractionManager.instance.SetInteractionMode(E_InteractionMode.Interacting);
            else
                InteractionManager.instance.SetInteractionMode(E_InteractionMode.NotInteracting);
        }

        isTransitioning = true;
        cameraNode.SetSelected(true);
        currentCameraMode = cameraNode.MyCameraMode;
        currentNode = cameraNode;
        transform.LookAt(cameraNode.transform);
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

    private void ActionDetermined(E_Action action, Interactable interactable)
    {
        if (action == E_Action.ZoomIn)
        {
            if (interactable is ZoomInteractable)
                SetCameraNode(interactable.GetComponent<CameraNode>());
            else
                MoveToNextNode();
        }

        else if(action == E_Action.Zooming)
        {
            if (InputProcessor.instance)
            {
                List<ClickInput> clickInputs = InputProcessor.instance.GetClickInputs().Where(input => input.FingerId != -1).ToList();

                if (clickInputs.Count > 1)
                {
                    touchDistance = Vector3.Distance(clickInputs[0].StartPosition, clickInputs[1].StartPosition);
                    InputProcessor.instance.onClickRelease += ClickRelease;
                }
            }
        }

        else if (action == E_Action.ZoomOut)
        {
            if (InputProcessor.instance)
            {
                List<ClickInput> clickInputs = InputProcessor.instance.GetClickInputs().Where(input => input.FingerId != -1).ToList();
                if (clickInputs.Count > 1)
                {
                    touchDistance = Vector3.Distance(clickInputs[0].StartPosition, clickInputs[1].StartPosition);
                    InputProcessor.instance.onClickRelease += ClickReleaseOut;
                }
            }
        }
    }

    private void ClickRelease(Vector2 position)
    {
        List<ClickInput> clickInputs = InputProcessor.instance.GetClickInputs().Where(input => input.FingerId != -1).ToList();

        if (touchDistance * pinchRatio < Vector3.Distance(clickInputs[0].StartPosition, position))
            MoveToNextNode();
        else if (touchDistance / pinchRatio > Vector3.Distance(clickInputs[0].StartPosition, position))
            MoveToPreviousNode();

        InputProcessor.instance.onClickRelease -= ClickRelease;
    }

    private void ClickReleaseOut(Vector2 position)
    {
        List<ClickInput> clickInputs = InputProcessor.instance.GetClickInputs();

        if (touchDistance / pinchRatio > Vector3.Distance(clickInputs[0].StartPosition, position))
            MoveToPreviousNode();

        InputProcessor.instance.onClickRelease -= ClickReleaseOut;
    }
}
