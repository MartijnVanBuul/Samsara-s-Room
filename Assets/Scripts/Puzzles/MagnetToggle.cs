using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetToggle : Interactable {

    public override void Interact(Vector2 position)
    {
        base.Interact(position);
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        if(transform.GetChild(0).gameObject.activeSelf)
            GetComponent<SpriteRenderer>().color = Color.white;
        else
            GetComponent<SpriteRenderer>().color = Color.grey;
    }

}
