using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour {

    private Vector3 startRotation;

    private bool isCorrect;

	// Use this for initialization
	void Start () {
        startRotation = transform.localEulerAngles;

        GetComponent<RotateInteraction>().onRotationChanged += RotationChanged;

        transform.localEulerAngles += Vector3.forward * (Random.Range(1, 4) * 90);
    }
	
	private void RotationChanged()
    {
        isCorrect = Vector3.Angle(transform.localEulerAngles, startRotation) < 15 || transform.localEulerAngles == startRotation;
        if (isCorrect)
        {
            Debug.Log(gameObject + " is correct");
            PointerManager.instance.CompletePointer();
        }
    }

    public bool GetIsCorrect()
    {


        return isCorrect;

    }
}
