using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum E_InteractionMode
{
    NotInteracting,
    Interacting,
    Undetermined
}


public enum E_Action
{
    Interact,
    Zooming,
    ZoomIn,
    ZoomOut,
    Rotate,
    Undetermined
}

public class InteractionManager : MonoBehaviour {

    public static InteractionManager instance;

    private int amountOfTouches;
    private List<ClickInput> clickInputs;
    private E_InteractionMode currentInteractionMode;

    private Interactable currentInteractable;

    private bool isDoubleTapped;
    private bool isMoved;

    public delegate void ActionDetermined(E_Action action, Interactable interactable);
    public ActionDetermined onActionDetermined;


    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        if (InputProcessor.instance)
        {
            InputProcessor.instance.onDoubleClick += DoubleTap;
            InputProcessor.instance.onClickRelease += ClickRelease;
        }

    }
	
	// Update is called once per frame
	void Update () {

    }

    private void LateUpdate()
    {
        isDoubleTapped = false;
    }

    public void SetInteractionMode(E_InteractionMode interactionMode)
    {
        currentInteractionMode = interactionMode;
    }

    public void SetInteractable(Interactable interactable = null)
    {
        currentInteractable = interactable;

        if (InputProcessor.instance)
            clickInputs = InputProcessor.instance.GetClickInputs();

        if (clickInputs != null)
            amountOfTouches = clickInputs.Count;

        if (onActionDetermined != null)
            onActionDetermined(DetermineAction(), currentInteractable);
    }

    private void DoubleTap(Vector2 position)
    {
        isDoubleTapped = true;
    }

    private void ClickRelease(Vector2 position)
    {
        currentInteractable = null;
    }

    private E_Action DetermineAction()
    {
        if (currentInteractionMode == E_InteractionMode.Interacting)
        {
            if (currentInteractable)
            {
                return E_Action.Interact;
            }
            else
            {
                if (amountOfTouches == 2)
                {
                    return E_Action.ZoomOut;
                }
                else
                {
                    return E_Action.Rotate;
                }
            }
        }
        else
        {
            if (amountOfTouches == 2)
            {
                return E_Action.Zooming;
            }
            else
            {
                if (isDoubleTapped)
                {
                    if (currentInteractable)
                    {
                        return E_Action.ZoomIn;
                    }
                }
                else
                {
                    return E_Action.Rotate;
                }
            }
        }


        return E_Action.Undetermined;
    }
}
