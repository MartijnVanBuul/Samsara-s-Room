using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAssignment : MonoBehaviour {

    [SerializeField]
    ClickInteraction phoneInteraction;

    public GameObject candlePrefab;

    Coroutine shakeCoroutine;

    int clicks = 0;

	// Use this for initialization
	void Start () {
        phoneInteraction.onObjectClicked += ClickPhone;
		
	}
	
	void ClickPhone()
    {

        if (clicks >= 3)
            return;

            if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakePhone());

        clicks++;

        if (clicks == 3)
            SpawnCandle();
    }

    IEnumerator ShakePhone()
    {

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-15f, 15));
        yield return new WaitForSeconds(.1f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-15f, 15));
        yield return new WaitForSeconds(.1f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    void SpawnCandle()
    {

        GameObject insCandle = Instantiate(candlePrefab, transform.position + new Vector3(0,0,.1f), transform.rotation);
        insCandle.GetComponent<Rigidbody>().AddExplosionForce(1, transform.position, 1, 1, ForceMode.Impulse);
    }
}
