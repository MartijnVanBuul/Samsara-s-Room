using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleValvesControl : MonoBehaviour
{
    private enum AxisName
    {
        X_Axis,
        Y_Axis,
        Z_Axis,
    };

    [SerializeField]
    private Transform puzzleTransform;
    [SerializeField]
    private DragRotatable xAxisRotatable;
    [SerializeField]
    private DragRotatable yAxisRotatable;
    [SerializeField]
    private DragRotatable zAxisRotatable;

    private Vector3 previousXAxisAngle;
    private Vector3 previousYAxisAngle;
    private Vector3 previousZAxisAngle;

    [SerializeField]
    GameObject xAxisVisuals;
    [SerializeField]
    GameObject xAxisVisuals2;

    [SerializeField]
    GameObject yAxisVisuals;
    [SerializeField]
    GameObject yAxisVisuals2;

    [SerializeField]
    GameObject zAxisVisuals;
    [SerializeField]
    GameObject zAxisVisuals2;

    private void Start()
    {

        SetVisuals(AxisName.X_Axis);

        xAxisRotatable.onRotationChanged += (x) => OnUpdateRotation(AxisName.X_Axis, x);
        yAxisRotatable.onRotationChanged += (x) => OnUpdateRotation(AxisName.Y_Axis, x);
        zAxisRotatable.onRotationChanged += (x) => OnUpdateRotation(AxisName.Z_Axis, x);

        previousXAxisAngle = xAxisRotatable.GetCurrentRotation();
        previousYAxisAngle = yAxisRotatable.GetCurrentRotation();
        previousZAxisAngle = zAxisRotatable.GetCurrentRotation();
    }

    private void OnUpdateRotation(AxisName axis, Vector3 rotationValue)
    {
        var deltaVector = Vector3.zero;

        SetVisuals(axis);

        switch (axis)
        {
            case AxisName.X_Axis:
                var currentPuzzleCubeAngles = puzzleTransform.localEulerAngles;

                deltaVector = rotationValue - previousXAxisAngle;
                currentPuzzleCubeAngles.x = currentPuzzleCubeAngles.x + deltaVector.z;
                previousXAxisAngle = rotationValue;

                puzzleTransform.Rotate(new Vector3(deltaVector.z, 0, 0));

            

                break;
            case AxisName.Y_Axis:
                currentPuzzleCubeAngles = puzzleTransform.localEulerAngles;

                deltaVector = rotationValue - previousYAxisAngle;
                currentPuzzleCubeAngles.y += deltaVector.z;
                previousYAxisAngle = rotationValue;

                puzzleTransform.Rotate(new Vector3(0, deltaVector.z, 0));

                break;
            case AxisName.Z_Axis:
                currentPuzzleCubeAngles = puzzleTransform.localEulerAngles;

                deltaVector = rotationValue - previousZAxisAngle;
                currentPuzzleCubeAngles.z += deltaVector.z;
                previousZAxisAngle = rotationValue;

                puzzleTransform.Rotate(new Vector3(0, 0, deltaVector.z));

                break;
        }
    }

    void SetVisuals(AxisName axis)
    {
        //zAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, .2f);
        //zAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, .2f);


        //yAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, .2f);
        //yAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, .2f);


        //xAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, .2f);
        //xAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, .2f);


        //switch (axis)
        //{
        //    case AxisName.X_Axis:
        //        xAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
        //        xAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);

        //        break;
        //    case AxisName.Y_Axis:
        //        yAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
        //        yAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);


        //        break;
        //    case AxisName.Z_Axis:
        //        zAxisVisuals.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);
        //        zAxisVisuals2.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 1);

        //        break;
        //}
        
    }
}
