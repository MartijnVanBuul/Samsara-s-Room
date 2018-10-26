using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAssignment : MonoBehaviour {

    [SerializeField]
    private DragRotatable clockhandHour;
    [SerializeField]
    private DragRotatable clockhandMinute;
    [SerializeField]
    private Sprite openClockSprite;
    [SerializeField]
    private GameObject windowKeyObject;

    [SerializeField]
    private float targetHourRotation;
    [SerializeField]
    private float targetMinuteRotation;
    [SerializeField]
    private float targetMargin = 5f;

    private bool isHourCorrect;
    private bool isMinuteCorrect;

    private bool isAssignmentComplete;



    // Use this for initialization
    void Start () {
        clockhandHour.onRotationUpdate += CheckHourValue;
        clockhandMinute.onRotationUpdate += CheckMinuteValue;
    }

    private void OnDestroy()
    {
        clockhandHour.onRotationUpdate -= CheckHourValue;
        clockhandMinute.onRotationUpdate -= CheckMinuteValue;
    }

    private void CheckHourValue(Vector3 rotation)
    {
        if ((rotation.z + 360 + targetMargin) % 360 > targetHourRotation && (rotation.z + 360 - targetMargin) % 360 < targetHourRotation)
        {
            isHourCorrect = true;
            CheckAssignmentComplete();
        }
        else
            isHourCorrect = false;
    }

    private void CheckMinuteValue(Vector3 rotation)
    {
        if ((rotation.z + 360 + targetMargin) % 360 > targetMinuteRotation && (rotation.z + 360 - targetMargin) % 360 < targetMinuteRotation)
        {
            isMinuteCorrect = true;
            CheckAssignmentComplete();
        }
        else
            isMinuteCorrect = false;
    }

    private void CheckAssignmentComplete()
    {
        if (isHourCorrect && isMinuteCorrect)
        {
            GetComponent<SpriteRenderer>().sprite = openClockSprite;
            windowKeyObject.SetActive(true);
            isAssignmentComplete = true;
        }
    }

    public bool GetAssignmentComplete()
    {
        return isAssignmentComplete;
    }
}
