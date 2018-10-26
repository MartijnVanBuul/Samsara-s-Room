using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TransitionZoomInteraction))]
public class ClickFade : DragInteractable
{

    private float progression;
    private float increaseOverTime = 0.02f;

    private bool isProgressing;

    private void Start()
    {
        if (InputProcessor.instance)
            InputProcessor.instance.onClickRelease += ProcessClickRelease;
    }

    private void OnDestroy()
    {
        if (InputProcessor.instance)
            InputProcessor.instance.onClickRelease -= ProcessClickRelease;
    }

    public override void Interact(Vector2 position)
    {
        isProgressing = true;
    }

    private void ProcessClickRelease(Vector2 position)
    {
        if (isProgressing)
            StartFade();
    }

    private void StartFade()
    {
        isProgressing = false;
        GetComponent<TransitionZoomInteraction>().StartCameraTransition();
        progression = 0;
    }

    public override void Update()
    {
        base.Update();

        if (isProgressing)
        {
            progression = Mathf.Clamp01(progression + increaseOverTime);

            if (progression == 1)
                StartFade();
            else if (progression > 0)
            {
                FadeManager.instance.SetProgression(progression);
                GetComponent<TransitionZoomInteraction>().SetZoom(progression);
            }
        }
    }
}
