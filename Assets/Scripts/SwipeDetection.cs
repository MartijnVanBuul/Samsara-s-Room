using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SwipeDirection
{
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    BottomLeft,
    Left,
    TopLeft,
    None
}

public class SwipeDetection : MonoBehaviour
{

    public static SwipeDetection instance;

    public delegate void SwipeEvent8Directional(E_SwipeDirection direction);
    public SwipeEvent8Directional onSwipe8Directions;

    public delegate void SwipeEvent4Directional(E_SwipeDirection direction);
    public SwipeEvent4Directional onSwipe4Directions;

    public delegate void SwipeEventRaw(Vector2 vector);
    public SwipeEventRaw onSwipeRaw;

    [SerializeField]
    private float totalInvalidateSwipeTime = 0.075f;

    private float minimalDistanceRatio = 0.05f;
    private float minimalTime = 0.1f;
    private float maximumTime = 0.5f;

    private float startTime;
    private Vector2 startPosition;

    private bool isSwiping;
    private Coroutine invalidatedSwipeCoroutine;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (InputProcessor.instance)
        {
            //InputProcessor.instance.onClick += ClickStarted;
            //InputProcessor.instance.onClickRelease += ClickEnded;
        }
    }

    private void OnDestroy()
    {
        if (InputProcessor.instance)
        {
            //InputProcessor.instance.onClick -= ClickStarted;
            //InputProcessor.instance.onClickRelease -= ClickEnded;
        }
    }

    private void ClickStarted(Vector2 startPosition)
    {
        startTime = Time.realtimeSinceStartup;
        this.startPosition = startPosition;
        isSwiping = true;
    }

    private void ClickEnded(Vector2 endPosition)
    {
        if (!isSwiping)
        {
            return;
        }

        if (Time.realtimeSinceStartup - startTime < maximumTime && Time.realtimeSinceStartup - startTime > minimalTime)
        {
            if (Vector2.Distance(startPosition, endPosition) > Mathf.Max(Screen.width, Screen.height) * minimalDistanceRatio)
            {
                if (onSwipe8Directions != null)
                    onSwipe8Directions(DetermineDirection(Vector3.Normalize(endPosition - startPosition)));

                if (onSwipe4Directions != null)
                    onSwipe4Directions(DetermineDirection(Vector3.Normalize(endPosition - startPosition), false));

                if (onSwipeRaw != null)
                    onSwipeRaw(endPosition - startPosition);
            }
        }

        isSwiping = false;
    }

    public float GetScreenRatio()
    {
        if (InputProcessor.instance)
            return Vector2.Distance(startPosition, InputProcessor.instance.GetPointerPosition()) / Mathf.Max(Screen.width, Screen.height);

        return 0;
    }

    /// <summary>
    /// Starts the InvalidateSwipeCoroutine to invalidate any and all swipes coming in the next so many seconds
    /// </summary>
    public void InvalidateCurrentSwipeMotion()
    {
        if (invalidatedSwipeCoroutine != null)
        {
            StopCoroutine(invalidatedSwipeCoroutine);
        }

        invalidatedSwipeCoroutine = StartCoroutine(InvalidateSwipeCoroutine());
    }

    /// <summary>
    /// Invalidates the swipe for the coming so many seconds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InvalidateSwipeCoroutine()
    {
        var currentCheckTime = 0f;
        while (currentCheckTime < totalInvalidateSwipeTime)
        {
            currentCheckTime += Time.deltaTime;

            isSwiping = false;

            yield return null;
        }
    }

    /// <summary>
    /// Returns the delta between the last input.mousePosition and the current input.mousePosition.
    /// If the axis as depicted by the positiveDirection is negatively changing we return a negative distance.
    /// </summary>
    public float GetDeltaSinceLastTouch(E_OrientationDirection positiveDirection)
    {
        //First we grab the pointerPositions
        var currentPointerPosition = InputProcessor.instance.GetPointerPosition();
        var lastPointerPosition = InputProcessor.instance.GetLastFramePointerPosition();

        //We then calculate the squaredDistance.
        var squaredDistance = Vector2.SqrMagnitude(currentPointerPosition - lastPointerPosition);

        //Then based on the positive direction we determine if the squaredDistance is negative or positive.
        if (positiveDirection == E_OrientationDirection.Top)
            squaredDistance = currentPointerPosition.y > lastPointerPosition.y ? -squaredDistance : squaredDistance;
        else if (positiveDirection == E_OrientationDirection.Bottom)
            squaredDistance = currentPointerPosition.y < lastPointerPosition.y ? -squaredDistance : squaredDistance;
        else if (positiveDirection == E_OrientationDirection.Left)
            squaredDistance = currentPointerPosition.x < lastPointerPosition.x ? -squaredDistance : squaredDistance;
        else if (positiveDirection == E_OrientationDirection.Right)
            squaredDistance = currentPointerPosition.x > lastPointerPosition.x ? -squaredDistance : squaredDistance;

        return squaredDistance;
    }

    /// <summary>
    /// Determining the direction of the swipe.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private E_SwipeDirection DetermineDirection(Vector2 direction, bool is8Ways = true)
    {
        if (is8Ways)
        {
            if (direction.x > Mathf.Sqrt(2))
                return E_SwipeDirection.Right;
            else if (direction.x < -Mathf.Sqrt(2))
                return E_SwipeDirection.Left;
            else if (direction.y > Mathf.Sqrt(2))
                return E_SwipeDirection.Top;
            else if (direction.y < -Mathf.Sqrt(2))
                return E_SwipeDirection.Bottom;
            else if (direction.x > 0 && direction.y > 0)
                return E_SwipeDirection.TopRight;
            else if (direction.x > 0 && direction.y < 0)
                return E_SwipeDirection.BottomRight;
            else if (direction.x < 0 && direction.y > 0)
                return E_SwipeDirection.TopLeft;
            else if (direction.x < 0 && direction.y < 0)
                return E_SwipeDirection.TopRight;
            else
                return E_SwipeDirection.None;
        }
        else
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && direction.x > 0)
                return E_SwipeDirection.Right;
            else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && direction.x < 0)
                return E_SwipeDirection.Left;
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && direction.y > 0)
                return E_SwipeDirection.Top;
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && direction.y < 0)
                return E_SwipeDirection.Bottom;
            else
                return E_SwipeDirection.None;
        }
    }

    public E_SwipeDirection GetSwipeDirection(bool is8Ways = true, bool byPassMaximumTime = false)
    {
        if (!isSwiping || !InputProcessor.instance || (!byPassMaximumTime && Time.realtimeSinceStartup - startTime > maximumTime))
            return E_SwipeDirection.None;

        else if (Vector2.Distance(startPosition, InputProcessor.instance.GetPointerPosition()) > Mathf.Max(Screen.width, Screen.height) * minimalDistanceRatio)
            return DetermineDirection(Vector3.Normalize(InputProcessor.instance.GetPointerPosition() - startPosition), is8Ways);

        return E_SwipeDirection.None;
    }
}
