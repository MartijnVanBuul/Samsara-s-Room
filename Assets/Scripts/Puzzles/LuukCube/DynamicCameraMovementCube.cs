using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraMovementCube : MonoBehaviour
{

    public static DynamicCameraMovementCube instance;

    [SerializeField]
    private bool enableMovement;
    [SerializeField]
    private float maximumMovement = 1f;
    [SerializeField]
    private float movementSpeed = 8f;
    [SerializeField]
    private bool enableRotation;
    [SerializeField]
    private float maximumRotation = 5f;
    [SerializeField]
    private float rotateSpeed = 8f;
    [SerializeField]
    private float zoomSpeed = 8f;

    private float width;
    private float height;

    private Vector3 baseRotation;
    private Vector3 offsetRotation;
    private Vector3 targetRotation;
    private Vector3 currentRotation;

    private Vector3 startPosition;
    private Vector3 offsetPosition;
    private Vector3 targetMovePosition;
    private Vector3 currentMovePosition;

    private bool isFreeToMove = true;
    private bool isZooming = false;
    private bool isDisplaced = false;

    [Space(10)]
    public Vector3 targetZoomPosition;

    GameObject endRotation;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;

        startPosition = currentMovePosition = transform.position;

        endRotation = new GameObject();

        if (SwipeDetection.instance)
            SwipeDetection.instance.onSwipe4Directions += OnSwipe;
    }

    private void OnSwipe(E_SwipeDirection direction)
    {
        if (!endRotation)
            endRotation = new GameObject();

        Vector3 currentRotationAxis = Vector3.zero;
        switch (direction)
        {
            case E_SwipeDirection.Left:
                endRotation.transform.Rotate(Vector3.up, 45, Space.World);
                break;
            case E_SwipeDirection.Right:
                endRotation.transform.Rotate(Vector3.up, -45, Space.World);
                break;
            case E_SwipeDirection.Top:
                endRotation.transform.Rotate(Vector3.left, -45, Space.World);
                break;
            case E_SwipeDirection.Bottom:
                endRotation.transform.Rotate(Vector3.left, 45, Space.World);
                break;



        }

        endRotation.transform.eulerAngles = new Vector3(Mathf.Round(endRotation.transform.eulerAngles.x / 45) * 45, Mathf.Round(endRotation.transform.eulerAngles.y / 45) * 45, Mathf.Round(endRotation.transform.eulerAngles.z / 45) * 45);

        //RotateBase(currentRotationAxis * 45);


    }

    // Update is called once per frame
    void Update()
    {
        if (InputProcessor.instance)
            MoveBasedOnMousePosition(InputProcessor.instance.GetPointerPosition());

        if (isZooming)
            MoveBasedOnZoomPosition(targetZoomPosition);
    }

    /// <summary>
    /// Method for setting the zoom position, this will also make the camera immediately move to that position.
    /// </summary>
    /// <param name="targetZoomPosition">The position the camera will move to.</param>
    public void SetZoomPosition(Vector3 targetZoomPosition)
    {
        this.targetZoomPosition = targetZoomPosition;
        isZooming = true;
    }

    public bool GetIsZooming()
    {
        return isZooming;
    }

    public Vector3 GetCurrentBaseRotation()
    {
        return baseRotation;
    }

    /// <summary>
    /// Method for resetting the zoom position.
    /// </summary>
    public void ResetZoom()
    {
        isZooming = false;
    }

    /// <summary>
    /// Method for moving and rotating the camera based on the zoom position.
    /// </summary>
    /// <param name="zoomPosition">The position of the camera when zoomed in.</param>
    private void MoveBasedOnZoomPosition(Vector3 zoomPosition)
    {
        transform.position = currentMovePosition;
        currentMovePosition += new Vector3(zoomPosition.x - currentMovePosition.x, zoomPosition.y - currentMovePosition.y, zoomPosition.z - currentMovePosition.z) * Mathf.Min(1, zoomSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Method for moving and rotating the camera based on the pointer.
    /// </summary>
    /// <param name="pointerPosition">The position of the pointer in the viewport.</param>
    private void MoveBasedOnMousePosition(Vector2 pointerPosition)
    {
        //Position
        if (enableMovement)
        {
            targetMovePosition = startPosition + offsetPosition + new Vector3(((pointerPosition.x - (width / 2)) / (width / 2)) * maximumMovement, ((pointerPosition.y - (height / 2)) / (height / 2)) * maximumMovement);
            currentMovePosition += new Vector3(targetMovePosition.x - currentMovePosition.x, targetMovePosition.y - currentMovePosition.y, targetMovePosition.z - currentMovePosition.z) * Mathf.Min(1, movementSpeed * Time.deltaTime);
            transform.position = currentMovePosition;
        }

        //Rotation
        if (enableRotation)
        {
            if(!endRotation)
                endRotation = new GameObject();

            targetRotation = baseRotation + offsetRotation + Vector3.Normalize(transform.InverseTransformDirection(Vector3.right) * ((pointerPosition.y - (height / 2)) / (height / 2)) * maximumRotation) + (transform.InverseTransformDirection(Vector3.up) * ((pointerPosition.x - (width / 2)) / (width / 2)) * maximumRotation);
            currentRotation += new Vector3(Mathf.DeltaAngle(currentRotation.x, targetRotation.x), Mathf.DeltaAngle(currentRotation.y, targetRotation.y), Mathf.DeltaAngle(currentRotation.z, targetRotation.z)) * Mathf.Min(1, rotateSpeed * Time.deltaTime);
            //transform.localEulerAngles = currentRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, endRotation.transform.rotation, 8 * Time.deltaTime);
        }
    }

    /// <summary>
    /// Method for setting the rotation of the camera.
    /// </summary>
    /// <param name="rotation">The rotation it is set on</param>
    public void SetBaseRotation(Vector3 rotation)
    {
        if (isDisplaced)
            return;
        if (isZooming)
            ResetZoom();
        else
            baseRotation = rotation;
    }

    /// <summary>
    /// Method for adding rotation to the base.
    /// </summary>
    /// <param name="rotation">The rotation added.</param>
    public void RotateBase(Vector3 rotation)
    {
        if (isDisplaced)
            return;
        else if (isZooming)
            ResetZoom();
        else
        {
            baseRotation += rotation;

            //if ((Mathf.RoundToInt(baseRotation.x) + 360) % 360 == 180)
            //{
            //    currentRotation = new Vector3(Mathf.Sign(baseRotation.x) * 90, (currentRotation.y - 180) % 360, currentRotation.z);
            //    baseRotation = new Vector3(0, (baseRotation.y - 180) % 360, baseRotation.z % 360);
            //}
            //else
                baseRotation = new Vector3(Mathf.Round((baseRotation.x % 360) / 45) * 45, Mathf.Round((baseRotation.y % 360) / 45) * 45, Mathf.Round((baseRotation.z % 360) / 450 * 45));



            if (!enableRotation)
                transform.localEulerAngles = baseRotation;
        }
    }

    /// <summary>
    /// Method for adding rotation to the base.
    /// </summary>
    /// <param name="rotation">The rotation added.</param>
    public void DisplaceBase(Vector3 displacement)
    {
        if (isDisplaced)
            return;
        else if (isZooming)
            ResetZoom();
        else
        {
            startPosition += displacement;

            transform.localPosition = startPosition;
        }
    }

    public void OffsetCamera(Vector3 displacement, Vector3 rotation)
    {
        isDisplaced = true;
        ResetZoom();

        offsetPosition = displacement;
        offsetRotation = rotation;

        transform.position = currentMovePosition = startPosition + offsetPosition;
        transform.localEulerAngles = currentRotation = baseRotation + offsetRotation;
    }

    public void ResetOffset()
    {
        offsetPosition = Vector3.zero;
        offsetRotation = Vector3.zero;

        transform.position = currentMovePosition = startPosition;
        transform.localEulerAngles = currentRotation = baseRotation;

        isDisplaced = false;
    }
}
