using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInteraction : Interaction
{
    [SerializeField]
    Vector3 zoomPosition;
    [SerializeField]
    private OrientationManager orientationManager;  //GOTTA REMOVE THIS, BUT DEPENDENT ON IT FOR CORRECT INTERACTIONS WITH ZOOMINTERACTION

    private bool isZoomedIn = false;

    public bool disableAfterUse = false;

    public override void PerformInteraction()
    {
        if (!isZoomedIn)
            DynamicCameraMovement.instance.SetZoomPosition(zoomPosition);
        else
        {
            orientationManager.StopTryingToChangeOrientation();
            DynamicCameraMovement.instance.SetBaseRotation(DynamicCameraMovement.instance.GetCurrentBaseRotation());
            //DynamicCameraMovement.instance.ResetZoom();
        }

        isZoomedIn = !isZoomedIn;

        base.PerformInteraction();

        if (disableAfterUse)
        {
            gameObject.SetActive(false);
        }
    }
}
