using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RollBallCubeManager : MonoBehaviour {

    public static RollBallCubeManager instance;

    [SerializeField]
    List<RollBallBehaviour> rollBalls;

    [SerializeField]
    List<GameObject> sides;

    public GameObject pointerPuzzle;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    private void OnEnable()
    {
        Physics.gravity = new Vector3(0, 0, 10);
        CameraRaycast.instance.SetRaycastMode(E_RaycastMode.All);
    }

    public void BallCompleted()
    {
        if (rollBalls.Count(ball => !ball.GetCompleted()) > 0)
        {
            Debug.Log("Complete");
            foreach (GameObject side in sides)
                StartCoroutine(moveSide(side));

            pointerPuzzle.SetActive(true);
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }

    }

    private IEnumerator moveSide(GameObject side)
    {
        float timer = 0;

        side.transform.SetParent(null);

        while (timer < 5)
        {
            side.transform.position += side.transform.up * timer * 2;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(side);
    }
}
