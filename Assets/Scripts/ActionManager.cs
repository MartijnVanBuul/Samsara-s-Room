using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ActionPriority
{
    ClickInteractionUI,
    DragInteractionUI,
    ClickInteractionPuzzle,
    DragInteractionPuzzle,
    ClickInteractionEnvironment,
    DragInteractionEnvironment,
    ClickInteractionNavigation,
    DragInteractionNavigation
}

public class ActionManager : MonoBehaviour {

    public static ActionManager instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
