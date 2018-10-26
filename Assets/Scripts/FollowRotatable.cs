using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotatable : MonoBehaviour {

	[SerializeField]
	private DragRotatable followableDragRotatable;

	void Start()
	{
		followableDragRotatable.onRotationUpdate += SetRotation;
	}

	private void SetRotation(Vector3 eulerAngles){
			transform.eulerAngles = eulerAngles;
	}
}
