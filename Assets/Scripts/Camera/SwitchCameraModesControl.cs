using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraModesControl : MonoBehaviour
{
    [SerializeField]
    private Camera cameraToAdapt;
    [SerializeField]
    private Transform transformToTranslate;
    [SerializeField]
    private SwitchCameraModeModel[] switchCameraModels;

    private int currentActiveIndex;
    private DynamicCameraMovement dynamicCameraMovement;

    /// <summary>
    /// We initialise the component on a currentActiveIndex of 0.
    /// </summary>
    public void Start()
    {
        currentActiveIndex = 0;

        if (cameraToAdapt != null)
        {
            dynamicCameraMovement = transformToTranslate.GetComponent<DynamicCameraMovement>();
        }
    }

    /// <summary>
    /// Switches between camera modes.
    /// </summary>
    public void GoToMode(int modeIndex, bool bypassActiveCheck = false)
    {
        if (((!bypassActiveCheck && currentActiveIndex != modeIndex) || bypassActiveCheck)
            && switchCameraModels != null && switchCameraModels.Length > modeIndex)
        {
            ApplyCameraModeModel(switchCameraModels[modeIndex]);
        }
    }

    /// <summary>
    /// Reset always goes to the first mode
    /// </summary>
    public void Reset()
    {
        GoToMode(0, bypassActiveCheck: true);
    }

    /// <summary>
    /// Applies the Camera Mode Model
    /// </summary>
    private void ApplyCameraModeModel(SwitchCameraModeModel switchCameraModeModel)
    {
        cameraToAdapt.orthographic = switchCameraModeModel.IsOrtographic;
        cameraToAdapt.orthographicSize = switchCameraModeModel.OrtographicSize;

        transformToTranslate.eulerAngles = switchCameraModeModel.RotationSettings;
        transformToTranslate.localPosition = switchCameraModeModel.PositionSettings;

        if (dynamicCameraMovement != null)
        {
            dynamicCameraMovement.targetZoomPosition = switchCameraModeModel.PositionSettings;
        }
    }
}

[Serializable]
public class SwitchCameraModeModel
{
    public bool IsOrtographic;
    public float OrtographicSize;
    public bool IsDynamicCameraMovementActive;
    public Vector3 PositionSettings;
    public Vector3 RotationSettings;
}
