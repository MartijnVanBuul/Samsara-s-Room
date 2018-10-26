using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTankBehaviour : MonoBehaviour {

    [SerializeField]
    private DragRotatable flowRotatable;
    [SerializeField]
    private DragRotatable waterLevelRotatable;
    [SerializeField]
    private Transform waterTransform;

    private float lastRotationValueFlow;
    private float lastRotationValueWater;

    private BuoyancyEffector2D myBuoyancyEffector;

    // Use this for initialization
    void Start () {
        flowRotatable.onRotationChanged += FlowRotatable_onRotationChanged;
        waterLevelRotatable.onRotationChanged += WaterLevelRotatable_onRotationChanged;

        myBuoyancyEffector = GetComponent<BuoyancyEffector2D>();
    }

    private void WaterLevelRotatable_onRotationChanged(Vector3 rotation)
    {
  
        myBuoyancyEffector.surfaceLevel = Mathf.Clamp(myBuoyancyEffector.surfaceLevel + Mathf.DeltaAngle(rotation.z, lastRotationValueWater) / 100f, -0.4f, 0.6f);
        waterTransform.localScale = new Vector3(0.48f, 0.3f * (myBuoyancyEffector.surfaceLevel + 0.4f), 1);
        waterTransform.localPosition = -Vector3.up * (myBuoyancyEffector.surfaceLevel + -0.6f) * -0.6f - Vector3.forward * 0.05f;
        lastRotationValueWater = rotation.z;
    }

    private void FlowRotatable_onRotationChanged(Vector3 rotation)
    {
        myBuoyancyEffector.flowMagnitude = Mathf.Clamp(myBuoyancyEffector.flowMagnitude + Mathf.DeltaAngle(rotation.z, lastRotationValueFlow) / 25f, -5f, 5f);
        lastRotationValueFlow = rotation.z;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
