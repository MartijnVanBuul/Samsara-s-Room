using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentActiveToggle : MonoBehaviour
{
    [SerializeField]
    private bool currentActiveState;

    private PerformInteractable performInteractable;

    private void Start()
    {
        performInteractable = GetComponent<PerformInteractable>();

        if (performInteractable != null)
        {
            performInteractable.OnPerformInteraction += OnToggleContentState;
        }

        ToggleContentState(currentActiveState);
    }

    private void OnDestroy()
    {
        if (performInteractable != null)
        {
            performInteractable.OnPerformInteraction -= OnToggleContentState;
        }
    }

    /// <summary>
    /// Gets called by PerformInteractable each time it performs an interaction.
    /// </summary>
    private void OnToggleContentState()
    {
        ToggleContentState(!currentActiveState);
    }

    /// <summary>
    /// Switches the active state of the content of this gameobject to the given new state.
    /// </summary>
    private void ToggleContentState(bool newState)
    {
        currentActiveState = newState;

        if (transform.childCount == 0)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            var childObject = transform.GetChild(i);
            if (childObject != null)
            {
                var pickupChild = childObject.GetComponent<PickUpInteractable>();
                if (pickupChild != null && !pickupChild.HasBeenTaken)
                {
                    childObject.gameObject.SetActive(currentActiveState);
                }
            }
        }
    }
}
