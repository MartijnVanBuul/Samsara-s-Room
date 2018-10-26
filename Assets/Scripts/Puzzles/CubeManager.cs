using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CubeManager : MonoBehaviour {

    [SerializeField]
    List<GameObject> doors = new List<GameObject>();
    [SerializeField]
    List<GameObject> topDoors = new List<GameObject>();
    [SerializeField]
    List<GameObject> rotatorButtons = new List<GameObject>();
    [SerializeField]
    List<GameObject> trays = new List<GameObject>();
    [SerializeField]
    List<ColorOverRotation> rotators = new List<ColorOverRotation>();
    [SerializeField]
    GameObject flickSwitchDoor;
    [SerializeField]
    GameObject restartButton;

    [SerializeField]
    GameObject trayDoor;

    List<bool> traysExtended = new List<bool>();

    public Text scoreText;
    private int currentScore;
    private int targetScore = 13;

    // Use this for initialization
    void Start () {
        foreach (GameObject tray in trays)
            traysExtended.Add(false);

    }

    public void CheckRotation()
    {
        if (rotators.Where(rot => !rot.currentlyCorrect).Count() == 0)
        {
            StartCoroutine(OpenDoor(0));
            StartCoroutine(OpenDoor(1));
            restartButton.SetActive(true);

            foreach (ColorOverRotation rot in rotators)
                Destroy(rot.GetComponent<DragRotatable>());
        }
    }

    public void ChangeScore(int difference)
    {
        if (currentScore != targetScore)
        {
            currentScore = Mathf.Clamp(currentScore + difference, 0, 999) ;
            scoreText.text = currentScore.ToString().PadLeft(3, '0');

            if (currentScore == targetScore)
                ShowRotaters();

        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator OpenDoor(int doorID)
    {
        Vector3 startRotation = doors[doorID].transform.localEulerAngles;
        Vector3 endRotation = new Vector3(-45, 0, 0);
        if(doorID == 1)
        {
            endRotation = new Vector3(45, 0, 0);
        }

        float openTimer = 0;

        while(openTimer < 1)
        {
            doors[doorID].transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, openTimer);
            openTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        doors[doorID].transform.localEulerAngles = endRotation;

    }

    public void OpenTrayDoor()
    {
        StartCoroutine(ScaleOverTime(trayDoor, trayDoor.transform.localScale, trayDoor.transform.localScale - trayDoor.transform.localScale.y * Vector3.up));
    }

    public void ShowFlickSwitches()
    {
        StartCoroutine(ScaleOverTime(flickSwitchDoor, flickSwitchDoor.transform.localScale, flickSwitchDoor.transform.localScale - flickSwitchDoor.transform.localScale.y * Vector3.up));
    }


    public void OpenTopDoor(int doorID)
    {
        StartCoroutine(ScaleOverTime(topDoors[doorID], topDoors[doorID].transform.localScale, topDoors[doorID].transform.localScale - topDoors[doorID].transform.localScale.y * Vector3.up));
    }

    public void ShowRotaters()
    {
        StartCoroutine(ShowRotatorButtons());
    }

    public void OpenTray(int trayID)
    {
        if(traysExtended[trayID])
            StartCoroutine(OpenTray(trays[trayID], -1));
        else
            StartCoroutine(OpenTray(trays[trayID], 1));

        traysExtended[trayID] = !traysExtended[trayID];
    }


    IEnumerator ScaleOverTime(GameObject objToScale, Vector3 beginScale, Vector3 endScale)
    {
        float scaleTimer = 0;

        while (scaleTimer < 1)
        {
            objToScale.transform.localScale = Vector3.Lerp(beginScale, endScale, scaleTimer);
            scaleTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        objToScale.transform.localScale = endScale;

    }

    IEnumerator ShowRotatorButtons()
    {
        for (int i = 0; i < rotatorButtons.Count; i++)
        {
            StartCoroutine(ScaleOverTime(rotatorButtons[i], rotatorButtons[i].transform.localScale, Vector3.one));
            yield return new WaitForSeconds(.1f);
        }

    }

    IEnumerator OpenTray(GameObject tray, int direction)
    {
        float moveTimer = 0;

        Vector3 beginPosition = tray.transform.localPosition;
        Vector3 endPosition = tray.transform.localPosition + Vector3.right * 0.3f * direction;

        while (moveTimer < 1)
        {
            tray.transform.localPosition = Vector3.Lerp(beginPosition, endPosition, moveTimer);
            moveTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tray.transform.localPosition = endPosition;
    }

}
