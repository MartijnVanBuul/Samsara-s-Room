using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIInteractable : Interactable {

    private Image myRenderer;
    public PickUpInteractable myInteractable;

    private void Start()
    {
        if (myRenderer == null)
            myRenderer = GetComponent<Image>();

        SelectionManager.instance.OnElementDeselected += ElementDeselected;
    }

    public override void Interact(Vector2 position)
    {
        base.Interact(position);

        if(SelectionManager.instance.GetSelectedElement() != this)
        {
            if(SelectionManager.instance.GetSelectedElement() != this)
                SelectionManager.instance.SelectElement(this);

            SelectInteractable();
        }
        else
        {
            if(SelectionManager.instance.GetSelectedElement() == this)
                SelectionManager.instance.DeselectElement();

            DeselectInteractable();
        }

    }

    public void SetInteractable(PickUpInteractable interactable)
    {
        myRenderer.enabled = true;
        myInteractable = interactable;
        myRenderer.sprite = interactable.PickUpSprite;
    }

    public void RemoveInteractable()
    {
        myRenderer.enabled = false;
        myInteractable = null;
        myRenderer.sprite = null;
    }

    private void ElementDeselected(UIInteractable elementDeselected){
        if(this == elementDeselected)
            DeselectInteractable();
    }

    private void SelectInteractable(){
        transform.parent.GetComponent<Image>().color = Color.grey;
    }

    private void DeselectInteractable(){
        transform.parent.GetComponent<Image>().color = Color.white;
    }

    private void ExternalDeselection(){

    }

}
