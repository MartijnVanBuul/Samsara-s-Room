using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_RaycastMode
{
    Single,
    Multiple,
    All
}

public class CameraRaycast : MonoBehaviour
{
    public static CameraRaycast instance;

    private E_RaycastMode myRaycastMode = E_RaycastMode.Single;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (InputProcessor.instance)
            InputProcessor.instance.onProcessClick += ProcessClick;
    }

    private void ProcessClick(Vector2 position)
    {
        if (Camera.main)
        {
            Ray screenPointToPosition = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;

            switch (myRaycastMode)
            {
                case E_RaycastMode.Single:
                    if (Physics.Raycast(screenPointToPosition.origin, screenPointToPosition.direction, out hit, Mathf.Infinity))
                    {
                        var hitCollider = hit.collider.GetComponent<Interactable>();

                        if (InteractionManager.instance)
                            InteractionManager.instance.SetInteractable(hitCollider);
                    }
                    else
                    {
                        if (InteractionManager.instance)
                            InteractionManager.instance.SetInteractable(null);
                    }
                    break;

                case E_RaycastMode.All:

                    RaycastHit[] hits = Physics.RaycastAll(screenPointToPosition.origin, screenPointToPosition.direction, Mathf.Infinity);
                    Interactable interactable = null;

                    foreach (RaycastHit rayHit in hits)
                    {
                        if (rayHit.collider.GetComponent<Interactable>() != null)
                        {
                            interactable = rayHit.collider.GetComponent<Interactable>();
                        }
                    }

                    if (InteractionManager.instance)
                        InteractionManager.instance.SetInteractable(interactable);

                    break;
            }
        }
    }

    public void SetRaycastMode(E_RaycastMode raycastMode)
    {
        myRaycastMode = raycastMode;
    }
}
