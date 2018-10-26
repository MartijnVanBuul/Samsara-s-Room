using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVisualiser : MonoBehaviour {

    public List<UIInteractable> inventoryRenderers;

    private List<KeyValuePair<PickUpInteractable, UIInteractable>> InteractableRendererPairs = new List<KeyValuePair<PickUpInteractable, UIInteractable>>();

	// Use this for initialization
	void Start () {

        if (Inventory.instance)
        {
            Inventory.instance.OnInteractableAdded += OnInteractableAdded;
            Inventory.instance.OnInteractableRemoved += OnInteractableRemoved;
        }

    }
	
	private void OnInteractableAdded(PickUpInteractable interactable)
    {
        foreach (UIInteractable UIInteractable in inventoryRenderers)
        {
            if (UIInteractable.myInteractable == null)
            {
                UIInteractable.SetInteractable(interactable);
                InteractableRendererPairs.Add(new KeyValuePair<PickUpInteractable, UIInteractable>(interactable, UIInteractable));

                break;
            }
        }
    }

    private void OnInteractableRemoved(PickUpInteractable interactable)
    {
        KeyValuePair<PickUpInteractable, UIInteractable> keyValuePair = InteractableRendererPairs.Find(pair => pair.Key == interactable);
        keyValuePair.Value.RemoveInteractable();
    }
}
