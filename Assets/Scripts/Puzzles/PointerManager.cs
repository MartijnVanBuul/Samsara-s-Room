using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointerManager : MonoBehaviour {

    public static PointerManager instance;

    [SerializeField]
    List<PointerBehaviour> pointers;

    public GameObject GravityPuzzle;

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
    
    public void CompletePointer()
    {
        if (pointers.Count(pointer => !pointer.GetIsCorrect()) == 0)
        {
            Debug.Log("Complete");
            foreach (PointerBehaviour side in pointers)
                StartCoroutine(moveSide(side.gameObject));

        }
    }

    private IEnumerator moveSide(GameObject side)
    {
        yield return new WaitForSeconds(1);
        GravityPuzzle.SetActive(true);

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
