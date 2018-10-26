using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBallSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private float minHeight = 0.5f;
    private GameObject currentBall;

    [SerializeField]
    private float spawnDelay = 3;
    private float spawnTimer;

    // Use this for initialization
    void Start() {
        SpawnBall();
    }

    // Update is called once per frame
    void Update() {

        spawnTimer += Time.deltaTime;

        if(spawnTimer > spawnDelay)
        {
            SpawnBall();
            spawnTimer -= spawnDelay;
        }

    }

    private void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        currentBall.GetComponent<PhysicsBall>().SetBallType((E_BallType)Random.Range(0, 2));

    }

}
