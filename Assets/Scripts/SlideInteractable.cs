using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInteractable : Interactable {

    private float currentPosition;
    private Vector2 lastPosition;

    public GameObject rotateCeiling;

	// Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (IsInteracting)
        {
            if (lastPosition == Vector2.zero)
                lastPosition = Input.mousePosition;

            currentPosition = Mathf.Clamp(currentPosition + (Input.mousePosition.x - lastPosition.x) / -500, -0.4f, 0.4f);
            transform.localPosition = new Vector3(currentPosition, 0, 4.5f);

            if (currentPosition == -0.4f)
                rotateCeiling.transform.localEulerAngles = new Vector3(0, 60, 0);

            lastPosition = Input.mousePosition;
        }
	}
}
