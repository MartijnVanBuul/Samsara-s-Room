using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DynamicCameraMovement))]
public class DynamicCameraMovementEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DynamicCameraMovement dynamicCameraMovement = (DynamicCameraMovement)target;

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Zoom"))
        {
            dynamicCameraMovement.SetZoomPosition(dynamicCameraMovement.targetZoomPosition);
        }

        if (GUILayout.Button("Reset"))
        {
            dynamicCameraMovement.ResetZoom();
        }
        GUILayout.EndHorizontal();
    }
}
#endif

