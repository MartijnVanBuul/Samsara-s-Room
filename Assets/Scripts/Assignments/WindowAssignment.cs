using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowAssignment : MonoBehaviour {

    [SerializeField]
    private PlaceInteraction windowLock;

    [SerializeField]
    private ClickInteraction portal;
    [SerializeField]
    private GameObject portalFX;
    [SerializeField]
    private GameObject painting;
    [SerializeField]
    private GameObject paintingFoldable;
    [SerializeField]
    private GameObject windowZoomInteraction;
    [SerializeField]
    private GameObject portalZoomInteraction;

    [SerializeField]
    private GameObject leftDoor;
    [SerializeField]
    private GameObject rightDoor;

    [SerializeField]
    BookPro paintingScroll;

    [SerializeField]
    float doorOpenSpeed = 5;

    private bool isAssignmentComplete;

    bool paintingOpened = false;
    bool fishOffered = false;


    // Use this for initialization
    void Start () {
        windowLock.onObjectPlaced += RemoveLock;
        portal.onObjectClicked += OpenPortal;
        paintingScroll.OnFlip.AddListener(PaintingMoved);
    }

    void RemoveLock()
    {
        windowZoomInteraction.SetActive(false);
        windowLock.gameObject.SetActive(false);

        StartCoroutine(RotateWindowDoors(leftDoor, leftDoor.transform.eulerAngles, new Vector3(leftDoor.transform.eulerAngles.x, 270, leftDoor.transform.eulerAngles.z)));
        StartCoroutine(RotateWindowDoors(rightDoor, rightDoor.transform.eulerAngles, new Vector3(rightDoor.transform.eulerAngles.x, -90, rightDoor.transform.eulerAngles.z)));

        painting.SetActive(false);
        paintingFoldable.SetActive(true);

    }

    void PaintingMoved()
    {
        paintingScroll.interactable = false;
        paintingOpened = true;
        EnablePortal();
    }

    void EnablePortal()
    {
        if(paintingOpened && fishOffered)
            portal.GetComponent<Collider>().enabled = true;
    }

    public void FishOffered()
    {
        fishOffered = true;
        EnablePortal();
    }

    IEnumerator RotateWindowDoors(GameObject objToRotate, Vector3 startRotation, Vector3 endRotation)
    {

        float lerpTimer = 0;
        while (lerpTimer < 1)
        {
            objToRotate.transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, lerpTimer);
            lerpTimer += Time.deltaTime * doorOpenSpeed;
            yield return new WaitForEndOfFrame();
        }

        objToRotate.transform.eulerAngles = endRotation;
    }

    private void CheckAssignmentComplete()
    {
        isAssignmentComplete = true;

    }

    void OpenPortal()
    {
        portal.GetComponent<Animator>().enabled = true;
        portalFX.SetActive(true);
        Invoke("EnablePortalZoomInteraction", 1);
    }

    void EnablePortalZoomInteraction()
    {
        portalZoomInteraction.SetActive(true);
        CheckAssignmentComplete();
    }

    public bool GetAssignmentComplete()
    {
        return isAssignmentComplete;
    }
}
