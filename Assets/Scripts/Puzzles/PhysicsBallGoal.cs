using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBallGoal : MonoBehaviour {

    [SerializeField]
    private E_BallType ballType;

    private bool isGoalCompleted;

    public E_BallType GetBallType()
    {
        return ballType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PhysicsBall>())
        {
            if (other.GetComponent<PhysicsBall>().GetBallType() == ballType)
                isGoalCompleted = true;
            else
                Destroy(other.gameObject);
        }
    }
}
