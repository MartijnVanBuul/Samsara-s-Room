using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public static SelectionManager instance;

    private UIInteractable selectedElement;

    public delegate void ElementSelectedEvent(UIInteractable selectedInteractable);
    public ElementSelectedEvent OnElementSelected;

    public delegate void ElementDeselectedEvent(UIInteractable deselectedInteractable);
    public ElementDeselectedEvent OnElementDeselected;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void SelectElement(UIInteractable interactable)
    {
        if (selectedElement != interactable)
        {
            DeselectElement();
            selectedElement = interactable;
        }

        if(OnElementSelected != null)
            OnElementSelected(interactable);

        Debug.Log("Selected element is " + interactable.name);
    }

    public void DeselectElement()
    {
        if(OnElementDeselected != null)
            OnElementDeselected(selectedElement);
            
        selectedElement = null;
    }

    public UIInteractable GetSelectedElement(){
        return selectedElement;
    }
}
