using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPuzzle3D : MonoBehaviour {

	[SerializeField]
	private List<Transform> puzzlePath;

	[SerializeField]
	private Transform stickTransform;

	private LineRenderer myLineRenderer;
	private int indexCurrentPuzzleIndex = 0;

	private bool isPuzzleCompleted;

	// Use this for initialization
	void Start () {
		myLineRenderer = GetComponent<LineRenderer>();

		myLineRenderer.positionCount = puzzlePath.Count * 2 + 1;

		for(int index = 0; index < puzzlePath.Count - 1; index++){
			myLineRenderer.SetPosition(index * 2, puzzlePath[index].localPosition);
			myLineRenderer.SetPosition(index * 2 + 1, puzzlePath[index].localPosition + puzzlePath[index + 1].localPosition);
		}

		myLineRenderer.SetPosition(puzzlePath.Count * 2, puzzlePath[puzzlePath.Count - 1].transform.localPosition);
	}
	
	// Update is called once per frame
	void Update () {
		if(!isPuzzleCompleted){
			if(indexCurrentPuzzleIndex > 0 && puzzlePath[indexCurrentPuzzleIndex - 1].position.y < stickTransform.position.y - 0.5)
			{
				stickTransform.position = puzzlePath[indexCurrentPuzzleIndex - 1].position;
				indexCurrentPuzzleIndex--;
			}
			else if(indexCurrentPuzzleIndex < puzzlePath.Count - 1 && puzzlePath[indexCurrentPuzzleIndex + 1].position.y < stickTransform.position.y - 0.5)
			{
				stickTransform.position = puzzlePath[indexCurrentPuzzleIndex + 1].position;
				indexCurrentPuzzleIndex++;

				if(indexCurrentPuzzleIndex == puzzlePath.Count - 1)
				{
					isPuzzleCompleted = true;
                    FindObjectOfType<CubeAssignment>().CubePuzzleComplete();
                    stickTransform.GetComponent<Renderer>().material.color = Color.green;
				}
			}		
		}

	}
}
