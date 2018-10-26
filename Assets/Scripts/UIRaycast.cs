using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRaycast : MonoBehaviour {

    void Start()
    {
        //if (InputProcessor.instance)
        //    InputProcessor.instance.onClick += ProcessClick;
    }

    private void OnDestroy()
    {
        //if (InputProcessor.instance)
        //    InputProcessor.instance.onClick -= ProcessClick;
    }

    private void ProcessClick(Vector2 position)
    {
        if (Camera.main)
        {
            //Code to be place in a MonoBehaviour with a GraphicRaycaster component
            GraphicRaycaster graphicRaycaster = this.GetComponent<GraphicRaycaster>();

            if (!graphicRaycaster)
                return;

            //Create the PointerEventData with null for the EventSystem
            PointerEventData eventData = new PointerEventData(null);
            //Set required parameters, in this case, mouse position
            eventData.position = position;
            //Create list to receive all results
            List<RaycastResult> results = new List<RaycastResult>();
            //Raycast it
            graphicRaycaster.Raycast(eventData, results);

            foreach (RaycastResult hit in results)
                if (hit.gameObject.GetComponent<Interactable>() != null)
                    hit.gameObject.GetComponent<Interactable>().Interact(position);
        }

    }
}
