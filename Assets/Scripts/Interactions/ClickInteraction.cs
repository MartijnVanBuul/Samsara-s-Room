using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickInteraction : Interaction {

    public delegate void ObjectClicked();
    public ObjectClicked onObjectClicked;

    public override void PerformInteraction()
    {
        base.PerformInteraction();

        ClickObject();
    }

    private void ClickObject()
    {
        Debug.Log("Clicked " + gameObject.name);
        if (onObjectClicked != null)
            onObjectClicked();
    }
}
