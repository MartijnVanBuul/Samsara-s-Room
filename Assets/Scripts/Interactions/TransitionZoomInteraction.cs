using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionZoomInteraction : Interaction
{
    public delegate void ZoomInteraction(bool isZooming);
    public ZoomInteraction onZoomInteraction;

    [SerializeField]
    private float fadeTime = 0.3f;

    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 startRotation;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private Vector3 targetRotation;

    [SerializeField]
    private Vector3 startPositionTransitioned;
    [SerializeField]
    private Vector3 startRotationTransitioned;
    [SerializeField]
    private Vector3 targetPositionTransitioned;
    [SerializeField]
    private Vector3 targetRotationTransitioned;

    private bool isDisplaced = false;
    private SwitchCameraModesControl switchCameraModesControl;

    public bool allowReset = true;

    private void Start()
    {
        switchCameraModesControl = GetComponent<SwitchCameraModesControl>();

        if (SwipeDetection.instance && allowReset)
            SwipeDetection.instance.onSwipe4Directions += ResetTransition;
        if (InputProcessor.instance && allowReset)
            InputProcessor.instance.onDoubleClick += ResetTransition;
    }

    private void OnDestroy()
    {
        if (SwipeDetection.instance && allowReset)
            SwipeDetection.instance.onSwipe4Directions -= ResetTransition;
        if (InputProcessor.instance && allowReset)
            InputProcessor.instance.onDoubleClick -= ResetTransition;
    }

    private void ResetTransition(Vector2 position)
    {
        if (DynamicCameraMovement.instance && isDisplaced)
            StartCoroutine(ResetCameraOffset());
    }

    private void ResetTransition(E_SwipeDirection direction)
    {
        if (DynamicCameraMovement.instance && isDisplaced)
            StartCoroutine(ResetCameraOffset());
    }

    public override void PerformInteraction()
    {
        base.PerformInteraction();

        StartCameraTransition();
    }

    public void StartCameraTransition(bool skipFade = false)
    {
        if (DynamicCameraMovement.instance && !isDisplaced)
            StartCoroutine(TransitionCameraOffset(skipFade));
    }

    public void SetZoom(float progression)
    {
        DynamicCameraMovement.instance.SetZoomPosition(startPosition + progression * (targetPosition - startPosition));
    }

    private IEnumerator TransitionCameraOffset(bool skipFade = false)
    {
        if(onZoomInteraction != null)
            onZoomInteraction(true);

        if (!skipFade)
        {
            DynamicCameraMovement.instance.SetZoomPosition(targetPosition);

            FadeManager.instance.StartFade(1, fadeTime);
            yield return new WaitForSeconds(fadeTime);

            DynamicCameraMovement.instance.ResetZoom();
            yield return new WaitForSeconds(fadeTime);
        }

        DynamicCameraMovement.instance.OffsetCamera(startPositionTransitioned, startRotationTransitioned);

        DynamicCameraMovement.instance.SetZoomPosition(targetPositionTransitioned);

        if (switchCameraModesControl != null)
        {
            switchCameraModesControl.GoToMode(1);
        }

        FadeManager.instance.StartFade(-1, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        isDisplaced = true;
    }

    private IEnumerator ResetCameraOffset()
    {
        if (onZoomInteraction != null)
            onZoomInteraction(false);

        //This "switchCameraModesControl" can be cleaned further by putting it into an event, instead of a component.
        //By placing it into an event the responsibility is shifted from this class to the one subscribing to the event.
        if (switchCameraModesControl == null)
        {
            DynamicCameraMovement.instance.ResetZoom();
        }

        FadeManager.instance.StartFade(1, fadeTime);
        yield return new WaitForSeconds(fadeTime);

        if (switchCameraModesControl != null)
        {
            switchCameraModesControl.Reset();
            DynamicCameraMovement.instance.ResetZoom();
        }

        DynamicCameraMovement.instance.ResetOffset();
        yield return new WaitForSeconds(fadeTime);

        FadeManager.instance.StartFade(-1, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        isDisplaced = false;
    }
}
