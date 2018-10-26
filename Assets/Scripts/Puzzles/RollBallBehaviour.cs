using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBallBehaviour : MonoBehaviour {

    private bool isCompleted;
    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            isCompleted = true;
            Debug.Log(gameObject + " is completed");
            RollBallCubeManager.instance.BallCompleted();
        }
    }

    public bool GetCompleted()
    {
        return isCompleted;
    }
}
