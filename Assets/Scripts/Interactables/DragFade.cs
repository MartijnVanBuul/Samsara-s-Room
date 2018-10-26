using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransitionZoomInteraction))]
public class DragFade : DragInteractable
{

    [SerializeField]
    private float screenRatioNeeded = 0.25f;

    private float progression;
    private float decay = 0.02f;

    public override void Update()
    {
        base.Update();


        if (isBeingDragged && currentPositionWorld != Vector2.zero)
            progression = Mathf.Clamp01((startPosition.y - currentPosition.y) / (Screen.height * screenRatioNeeded));
        else
            progression = Mathf.Clamp01(progression - decay);

        if (progression > 0)
        {
            FadeManager.instance.SetProgression(progression);
            GetComponent<TransitionZoomInteraction>().SetZoom(progression);
        }


    }

    protected override void DragReleased(Vector2 position)
    {
        if (progression == 1)
            GetComponent<TransitionZoomInteraction>().StartCameraTransition(true);
        else
            Reset();

        progression = 0;

        base.DragReleased(position);

    }

    private void Reset()
    {
        DynamicCameraMovement.instance.ResetZoom();
        FadeManager.instance.StartFade(-1, 0.3f);
    }
}
