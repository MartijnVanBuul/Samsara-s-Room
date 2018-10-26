using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveInPoint : MonoBehaviour {

    public float randomRadius = 5;
    public float moveSpeed = 5;

    Vector3 basePos;

	// Use this for initialization
	void Start () {
        basePos = transform.position;
        StartCoroutine(Move());
	}
	
	IEnumerator Move()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = basePos + (Random.insideUnitSphere * randomRadius);

        float moveTimer = 0;

        while(moveTimer < 1)
        {
            moveTimer += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(startPos, targetPos, moveTimer);
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPos;

        yield return Move();

    }
}
