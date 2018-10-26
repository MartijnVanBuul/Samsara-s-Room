using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAssignment : MonoBehaviour
{
    
    [SerializeField]
    List<PlaceInteraction> firesLitList = new List<PlaceInteraction>();

    [SerializeField]
    PlaceInteraction fishSocket;

    private bool isAssignmentComplete;

    int firesLit = 0;
    bool fishPlaced = false;

    [SerializeField]
    List<GameObject> mainLights = new List<GameObject>();

    [SerializeField]
    BookPro carpetBook;

    // Use this for initialization
    void Start()
    {
        foreach (PlaceInteraction litCandle in firesLitList)
        {
            litCandle.onObjectPlaced += IncreaseLitCandles;
            litCandle.onObjectPlaced += CheckAssignmentComplete;

        }

        fishSocket.onObjectPlaced += FishPlaced;
        fishSocket.onObjectPlaced += CheckAssignmentComplete;

        carpetBook.OnFlip.AddListener(carpetOpen);

    }
    private void IncreaseLitCandles()
    {
        foreach (GameObject go in mainLights)
        {
            go.SetActive(false);
        }

        firesLit++;
    }
    private void FishPlaced()
    {
        fishPlaced = true;
    }

    void carpetOpen()
    {
        carpetBook.interactable = false;
    }

    private void CheckAssignmentComplete()
    {
        if (fishPlaced && firesLit >= 5)
        {
            isAssignmentComplete = true;
            FindObjectOfType<WindowAssignment>().FishOffered();
        }
    }

    public bool GetAssignmentComplete()
    {
        return isAssignmentComplete;
    }
}
