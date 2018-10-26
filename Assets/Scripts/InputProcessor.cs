using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct ClickInput
{
    public int FingerId;
    public float StartTime;
    public Vector2 StartPosition;

    public ClickInput(int fingerId, float startTime, Vector2 startPosition)
    {
        this.FingerId = fingerId;
        this.StartTime = startTime;
        this.StartPosition = startPosition;
    }
}

public class InputProcessor : MonoBehaviour
{

    public static InputProcessor instance;

    private float doubleClickTime = 0.2f;
    private float lastClickTime;

    private Vector2 lastFramePointerPosition;
    private Vector2 currentFramePointerPosition;

    private bool isClickedThisFrame;

    List<ClickInput> clickInputs = new List<ClickInput>();

    public delegate void ProcessClickEvent(Vector2 screenPoint);
    public ProcessClickEvent onProcessClick;

    public delegate void DoubleClickEvent(Vector2 screenPoint);
    public DoubleClickEvent onDoubleClick;

    public delegate void ClickReleaseEvent(Vector2 screenPoint);
    public ClickReleaseEvent onClickRelease;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        isClickedThisFrame = false;

        //if (Input.GetMouseButtonDown(0))
        //    ProcessClick(Input.mousePosition);

        //if (Input.GetMouseButtonUp(0))
        //    Release(Input.mousePosition);

        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    ProcessClick(touch.position, touch.fingerId);
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Canceled:
                    Release(touch.position, touch.fingerId);
                    break;
                case TouchPhase.Ended:
                    Release(touch.position, touch.fingerId);
                    break;
            }
        }

        lastFramePointerPosition = currentFramePointerPosition;
        currentFramePointerPosition = Input.mousePosition;
    }

    private void ProcessClick(Vector2 position, int id = -1)
    {
        clickInputs.Add(new ClickInput(id, Time.realtimeSinceStartup, position));

        isClickedThisFrame = true;

        if (Time.realtimeSinceStartup - lastClickTime < doubleClickTime)
            if (onDoubleClick != null)
                onDoubleClick(position);

        if (onProcessClick != null)
            onProcessClick(position);

        lastClickTime = Time.realtimeSinceStartup;
    }

    private void Release(Vector2 position, int id = -1)
    {
        if(clickInputs.FindIndex(touchInput => touchInput.FingerId == id) != -1)
            clickInputs.RemoveAt(clickInputs.FindIndex(touchInput => touchInput.FingerId == id));

        if (onClickRelease != null)
            onClickRelease(position);
    }

    /// <summary>
    /// Method returning the position of the mouse/touch on the screen.
    /// </summary>
    /// <returns>The position of interaction.</returns>
    public Vector2 GetPointerPosition()
    {
        return currentFramePointerPosition;
    }

    /// <summary>
    /// Method that returns the difference in positioning between this and last frame.
    /// </summary>
    /// <returns>Difference between current and last frame.</returns>
    public Vector2 GetDeltaPosition()
    {
        if (isClickedThisFrame)
            return Vector2.zero;

        return currentFramePointerPosition - lastFramePointerPosition;
    }

    /// <summary>
    /// Method returning the position of the mouse/touch on the screen, of previous frame.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLastFramePointerPosition()
    {
        return lastFramePointerPosition;
    }

    /// <summary>
    /// Returning the click inputs.
    /// </summary>
    /// <returns>the click inputs</returns>
    public List<ClickInput> GetClickInputs()
    {
        return clickInputs;
    }
}
