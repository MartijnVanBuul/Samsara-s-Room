using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInteractable : Interactable {

    protected Vector2 startPosition;
    protected Vector2 lastPosition;
    protected Vector2 currentPosition;
    protected Vector2 endPosition;

    protected Vector2 startPositionWorld;
    protected Vector2 lastPositionWorld;
    protected Vector2 currentPositionWorld;
    protected Vector2 endPositionWorld;

    protected bool isBeingDragged;

    public virtual void Update()
    {
        if (isBeingDragged && Camera.main && InputProcessor.instance)
        {
            if (currentPosition != Vector2.zero)
                lastPosition = currentPosition;
            if (currentPositionWorld != Vector2.zero)
                lastPositionWorld = currentPositionWorld;

            currentPosition = InputProcessor.instance.GetPointerPosition();
            currentPositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(currentPosition.x, currentPosition.y, transform.position.z - Camera.main.transform.position.z));
        }
    }


    public virtual Vector2 GetCurrentPosition()
    {
        return currentPositionWorld;
    }

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        isBeingDragged = true;

        if (InputProcessor.instance)
            InputProcessor.instance.onClickRelease += DragReleased;

        startPosition = position;
        startPositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, transform.position.z - Camera.main.transform.position.z));
    }

    protected virtual void DragReleased(Vector2 position)
    {
        Reset();

        if (InputProcessor.instance)
            InputProcessor.instance.onClickRelease -= DragReleased;
    }

    private void Reset()
    {
        isBeingDragged = false;

        lastPosition = Vector2.zero;
        startPosition = Vector2.zero;
        currentPosition = Vector2.zero;
        endPosition = Vector2.zero;

        lastPositionWorld = Vector2.zero;
        startPositionWorld = Vector2.zero;
        currentPositionWorld = Vector2.zero;
        endPositionWorld = Vector2.zero;
    }
}
