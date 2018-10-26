using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInteraction : Interaction {

    [Header("Place interaction properties")]
    public string ObjectName;

    public delegate void ObjectPlaced();
    public ObjectPlaced onObjectPlaced;

    public List<GameObject> ObjectsToEnable = new List<GameObject>();

    public override void PerformInteraction()
    {
        if (SelectionManager.instance)
        {
            if (SelectionManager.instance.GetSelectedElement())
            {
                if (SelectionManager.instance.GetSelectedElement().myInteractable.Name == ObjectName)
                {
                    PlaceObject(SelectionManager.instance.GetSelectedElement().myInteractable);

                    if (SelectionManager.instance.GetSelectedElement().myInteractable.consume)
                    {

                        if (Inventory.instance)
                            Inventory.instance.RemoveInteractable(SelectionManager.instance.GetSelectedElement().myInteractable);

                        SelectionManager.instance.DeselectElement();
                    }


                }
            }
        }

        base.PerformInteraction();
    }

    private void PlaceObject(PickUpInteractable interactable)
    {
        Debug.Log("Placed " + interactable.Name);
        if (onObjectPlaced != null)
            onObjectPlaced();


        foreach (GameObject objToEnable in ObjectsToEnable)
        {
            objToEnable.SetActive(true);
        }

    }
}
