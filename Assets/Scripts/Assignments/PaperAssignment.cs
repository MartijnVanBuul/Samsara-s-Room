using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperAssignment : MonoBehaviour
{

    [SerializeField]
    private PlaceInteraction paper;
    [SerializeField]
    private ClickInteraction keySketchPaper;
    [SerializeField]
    private ClickInteraction crumbledPaper;
    [SerializeField]
    private ClickInteraction crumbledOpenPaper;

    [SerializeField]
    private GameObject paperKey;

    private bool isAssignmentComplete;

    private PaperState currentState;
    enum PaperState
    {
        original,
        sketched,
        crumbled,
        openCrumbed
    }

    // Use this for initialization
    void Start()
    {
        paper.onObjectPlaced += PaperInteraction;
        keySketchPaper.onObjectClicked += PaperInteraction;
        crumbledPaper.onObjectClicked += PaperInteraction;

        // pencil > paper
        // show key drawing
        // click on paper with key
        // crumble paper
        // click crumbled paper
        // open paper
        // show key


    }
    void PaperInteraction()
    {
        if (currentState == PaperState.original)
            NextState(PaperState.sketched);
        else if (currentState == PaperState.sketched)
            NextState(PaperState.crumbled);
        else if (currentState == PaperState.crumbled)
            NextState(PaperState.openCrumbed);
    }

    void NextState(PaperState stateToSet)
    {
        currentState = stateToSet;

        switch (currentState)
        {
            case PaperState.original:

                break;
            case PaperState.sketched:
                paper.gameObject.SetActive(false);
                keySketchPaper.gameObject.SetActive(true);

                break;
            case PaperState.crumbled:
                keySketchPaper.gameObject.SetActive(false);
                crumbledPaper.gameObject.SetActive(true);
                break;
            case PaperState.openCrumbed:
                crumbledPaper.gameObject.SetActive(false);
                crumbledOpenPaper.gameObject.SetActive(true);
                paperKey.SetActive(true);
                CheckAssignmentComplete();
                break;

        }
    }

    private void CheckAssignmentComplete()
    {
        isAssignmentComplete = true;
    }

    public bool GetAssignmentComplete()
    {
        return isAssignmentComplete;
    }
}
