using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_OrientationDirection
{
    Top,
    Right,
    Bottom,
    Left,
    None
}

public class OrientationManager : MonoBehaviour
{

    [SerializeField]
    private float preemtiveRotation = 5f;
    [SerializeField]
    [Range(0, 1)]
    private float preemptiveThresholdProgressPercentage = 0.7f;
    [SerializeField]
    [Tooltip("Lower number is faster rotation, higher number is slower rotation")]
    private float preemptiveMaxRotationTresholdSquared = 35000;
    [SerializeField]
    private AnimationCurve preemptiveSnapCurve;
    [SerializeField]
    private bool isDetectingSwipe = true;
    [SerializeField]
    private bool isDetectingDoubleClick = true;


    bool isCheckingSwipe;
    bool isPreemptive;

    private bool lockProcessClick = false;

    private float currentPreemptiveRotationProgress = 0f;
    private Vector3 prePreemptiveRotationRotation;

    private Coroutine trackPointerPositionChangeCoroutine;

    private E_OrientationDirection preemptiveDirection = E_OrientationDirection.None;

    // Use this for initialization
    void Start()
    {
        //Adapt PreemptiveMaxRotationThrresholdSquared based on target resolution. This ensures the preemptive movements work properly.
        var xPortion = (float)Screen.width / 1920f;
        var yPortion = (float)Screen.height / 1080f;
        var totalChange = xPortion * yPortion;
        preemptiveMaxRotationTresholdSquared *= totalChange;

        if (InputProcessor.instance)
        {
            //InputProcessor.instance.onClick += ProcessClick;
            //InputProcessor.instance.onClickRelease += Reset;
            //InputProcessor.instance.onDoubleClick += ProcessDoubleClick;
        }
    }

    private void OnDestroy()
    {
        if (InputProcessor.instance)
        {
            //InputProcessor.instance.onClick -= ProcessClick;
            //InputProcessor.instance.onClickRelease -= Reset;
            //InputProcessor.instance.onDoubleClick -= ProcessDoubleClick;
        }
    }

    private void Update()
    {
        if (isCheckingSwipe && SwipeDetection.instance && DynamicCameraMovement.instance)
        {
            if (!DynamicCameraMovement.instance.GetIsZooming())
            {
                //Camera is first mirrored when swiping, then when continuing to swipe
                //It turns unmirrored, but the below direction is null.
                //Reason the direction turns to null after a bit is because the maximum swipe time is exceeded.
                E_OrientationDirection direction = ProcessDirection(SwipeDetection.instance.GetSwipeDirection(is8Ways: false, byPassMaximumTime: true)); //You can name parameters! Makes it easier to read for the people that didn't write the code :P
                if (!isPreemptive)
                {
                    if (SwipeDetection.instance.GetSwipeDirection(is8Ways: false, byPassMaximumTime: true) != E_SwipeDirection.None)
                    {
                        PreemptiveChangeOrientation(direction, trackPointerPositionChange: true);
                    }
                }
                else if (preemptiveDirection != direction)
                {
                    //Why the double reset?
                    //PreemptiveChangeOrientation(MirrorDirection(preemptiveDirection));
                    //PreemptiveChangeOrientation(direction);
                }
            }
        }
    }

    public void StopTryingToChangeOrientation()
    {
        if (trackPointerPositionChangeCoroutine != null)
        {
            StopCoroutine(trackPointerPositionChangeCoroutine);
        }

        currentPreemptiveRotationProgress = 0;
        lockProcessClick = true;
    }

    private void ProcessClick(Vector2 clickPosition)
    {
        if (!lockProcessClick)
        {
            isCheckingSwipe = true;
        }
    }

    private void Reset(Vector2 position)
    {
        if (trackPointerPositionChangeCoroutine != null)
        {
            StopCoroutine(trackPointerPositionChangeCoroutine);
        }

        //If it is preemptive we also check to see if the progress was over the threshold
        //If so we changeOrientation
        if (isPreemptive)
        {
            if (currentPreemptiveRotationProgress > preemptiveThresholdProgressPercentage)
            {
                ChangeOrientation(preemptiveDirection);
            }
            else
            {
                DynamicCameraMovement.instance.SetBaseRotation(prePreemptiveRotationRotation);  //Resets the Rotation of the Camera back to Before we started changing it.
            }
        }

        preemptiveDirection = E_OrientationDirection.None;
        isPreemptive = false;
        isCheckingSwipe = false;
        lockProcessClick = false;
        currentPreemptiveRotationProgress = 0;
    }

    /// <summary>
    /// Same as "ProcessDirection"
    /// </summary>
    private E_OrientationDirection MirrorDirection(E_OrientationDirection direction)
    {
        E_OrientationDirection changeOrientation = E_OrientationDirection.None;

        if (direction == E_OrientationDirection.Top)
            changeOrientation = E_OrientationDirection.Bottom;
        else if (direction == E_OrientationDirection.Bottom)
            changeOrientation = E_OrientationDirection.Top;
        else if (direction == E_OrientationDirection.Right)
            changeOrientation = E_OrientationDirection.Left;
        else if (direction == E_OrientationDirection.Left)
            changeOrientation = E_OrientationDirection.Right;

        return changeOrientation;
    }

    private void ProcessDoubleClick(Vector2 clickPosition)
    {
        if (!isDetectingDoubleClick)
            return;

        E_OrientationDirection changeOrientation = E_OrientationDirection.None;

        float xRatio = clickPosition.x / Screen.width;
        float yRatio = clickPosition.y / Screen.height;

        if (yRatio > 0.9f && yRatio > xRatio)
            changeOrientation = E_OrientationDirection.Top;
        else if (yRatio < 0.1f && yRatio < xRatio)
            changeOrientation = E_OrientationDirection.Bottom;
        else if (xRatio > 0.9f && xRatio > yRatio)
            changeOrientation = E_OrientationDirection.Right;
        else if (xRatio < 0.1f && xRatio < yRatio)
            changeOrientation = E_OrientationDirection.Left;

        ChangeOrientation(changeOrientation);
    }

    /// <summary>
    /// Same as MirrorDirection.
    /// </summary>
    private E_OrientationDirection ProcessDirection(E_SwipeDirection direction)
    {
        E_OrientationDirection changeOrientation = E_OrientationDirection.None;

        if (direction == E_SwipeDirection.Top)
            changeOrientation = E_OrientationDirection.Bottom;
        else if (direction == E_SwipeDirection.Bottom)
            changeOrientation = E_OrientationDirection.Top;
        else if (direction == E_SwipeDirection.Right)
            changeOrientation = E_OrientationDirection.Left;
        else if (direction == E_SwipeDirection.Left)
            changeOrientation = E_OrientationDirection.Right;

        return changeOrientation;
    }

    private void PreemptiveChangeOrientation(E_OrientationDirection direction, bool trackPointerPositionChange = false)
    {
        if (Camera.main && DynamicCameraMovement.instance)
        {
            isPreemptive = !isPreemptive;

            if (trackPointerPositionChange)
            {
                if (trackPointerPositionChangeCoroutine != null)
                {
                    StopCoroutine(trackPointerPositionChangeCoroutine);
                }

                trackPointerPositionChangeCoroutine = StartCoroutine(TrackPointerPositionChangeForOrientationChange(direction));
            }

            preemptiveDirection = direction;
        }
    }

    private IEnumerator TrackPointerPositionChangeForOrientationChange(E_OrientationDirection direction)
    {
        var totalDelta = 0f;
        var newRotationAddition = Vector3.zero;
        prePreemptiveRotationRotation = DynamicCameraMovement.instance.GetCurrentBaseRotation();

        while (true)
        {
            //We grab the current delta.
            var delta = SwipeDetection.instance.GetDeltaSinceLastTouch(direction);
            totalDelta += delta;

            //Then we calculate the currentProgress based on the totalDelta
            currentPreemptiveRotationProgress = totalDelta / preemptiveMaxRotationTresholdSquared;
            currentPreemptiveRotationProgress = currentPreemptiveRotationProgress > 1 ? 1f : currentPreemptiveRotationProgress;

            currentPreemptiveRotationProgress = preemptiveSnapCurve.Evaluate(currentPreemptiveRotationProgress);

            //Based on the direction we calculate a newRotationAddition
            if (direction == E_OrientationDirection.Top)
                newRotationAddition = Vector3.Lerp(Vector3.zero, Vector3.left * preemtiveRotation, currentPreemptiveRotationProgress);
            else if (direction == E_OrientationDirection.Bottom)
                newRotationAddition = Vector3.Lerp(Vector3.zero, Vector3.right * preemtiveRotation, currentPreemptiveRotationProgress);
            else if (direction == E_OrientationDirection.Right)
                newRotationAddition = Vector3.Lerp(Vector3.zero, Vector3.up * preemtiveRotation, currentPreemptiveRotationProgress);
            else if (direction == E_OrientationDirection.Left)
                newRotationAddition = Vector3.Lerp(Vector3.zero, Vector3.down * preemtiveRotation, currentPreemptiveRotationProgress);

            //Then we apply the newRotationAddition
            DynamicCameraMovement.instance.SetBaseRotation(prePreemptiveRotationRotation + newRotationAddition);

            yield return null;
        }
    }

    private void ChangeOrientation(E_OrientationDirection direction)
    {
        if (Camera.main && DynamicCameraMovement.instance)
        {
            float rotation = 90f;

            if (isPreemptive)
                rotation -= preemtiveRotation;

            if (direction == E_OrientationDirection.Top)
                DynamicCameraMovement.instance.RotateBase(Vector3.left * rotation);
            else if (direction == E_OrientationDirection.Bottom)
                DynamicCameraMovement.instance.RotateBase(Vector3.right * rotation);
            else if (direction == E_OrientationDirection.Right)
                DynamicCameraMovement.instance.RotateBase(Vector3.up * rotation);
            else if (direction == E_OrientationDirection.Left)
                DynamicCameraMovement.instance.RotateBase(Vector3.down * rotation);
        }
    }
}
