using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAssignment : MonoBehaviour {


    [SerializeField]
    ClickInteraction cubeInteraction;


    [SerializeField]
    GameObject startCube;


    [SerializeField]
    GameObject puzzleCube;

    [SerializeField]
    GameObject solvedCube;


    // Use this for initialization
    void Start () {
        cubeInteraction.onObjectClicked += ShowPuzzleCube;
	}
	
    void ShowPuzzleCube()
    {
        GameObject.Find("Inventory").SetActive(false);
        startCube.SetActive(false);
        puzzleCube.SetActive(true);
        GameObject.Find("FX_Portal").SetActive(false);

    }

    public void CubePuzzleComplete()
    {
        puzzleCube.SetActive(false);
        solvedCube.SetActive(true);

    }

}
