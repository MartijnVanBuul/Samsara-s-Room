using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOverRotation : MonoBehaviour {

    [SerializeField]
    private Gradient myGradient;

    [SerializeField]
    private float targetEuler;

    private MeshRenderer myRenderer;

    public bool currentlyCorrect = false;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();

        GetComponent<DragRotatable>().onRotationChanged += ColorOverRotation_onRotationChanged;
        myRenderer.material.color = myGradient.Evaluate(Mathf.Abs(Mathf.DeltaAngle(targetEuler, transform.localEulerAngles.y)) / 180f);

    }

    private void ColorOverRotation_onRotationChanged(Vector3 rotation)
    {
        myRenderer.material.color = myGradient.Evaluate(Mathf.Abs(Mathf.DeltaAngle(targetEuler, transform.localEulerAngles.y)) / 180f);
        currentlyCorrect = (Mathf.Abs(Mathf.DeltaAngle(targetEuler, transform.localEulerAngles.y)) / 180f < 0.1f);

        if (currentlyCorrect)
            FindObjectOfType<CubeManager>().CheckRotation();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
