using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BallType
{
    Red,
    Blue
}

public class PhysicsBall : MonoBehaviour {

    private E_BallType myBallType;
    private SpriteRenderer myRenderer;

    // Use this for initialization
    void Start () {
        myRenderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < 0.3f)
            Destroy(gameObject);
	}

    public void SetBallType(E_BallType ballType)
    {
        if(!myRenderer)
            myRenderer = GetComponent<SpriteRenderer>();

        myBallType = ballType;

        switch (myBallType)
        {
            case E_BallType.Blue:
                myRenderer.color = Color.blue;
                break;
            case E_BallType.Red:
                myRenderer.color = Color.red;
                break;
        }
    }

    public E_BallType GetBallType()
    {
        return myBallType;
    }
}
