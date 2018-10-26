using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public bool IsInteracting;

    public Vector2 StartPosition;

	// Use this for initialization
	public void Start () {
        if (InteractionManager.instance)
            InteractionManager.instance.onActionDetermined += ActionDetermined;

    }

    private void ActionDetermined(E_Action action, Interactable interactable)
    {
        if (action == E_Action.Interact && interactable == this)
        {
            if (InputProcessor.instance)
                InputProcessor.instance.onClickRelease += ClickRelease;

            StartPosition = Input.mousePosition;
            IsInteracting = true;
        }
    }

    private void ClickRelease(Vector2 position)
    {
        if (InputProcessor.instance)
            InputProcessor.instance.onClickRelease -= ClickRelease;

        IsInteracting = false;
    }
}
