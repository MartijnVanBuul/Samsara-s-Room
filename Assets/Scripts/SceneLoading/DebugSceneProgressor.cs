using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneProgressor : MonoBehaviour {

    //HACK: For development and debugging only.
    public bool DebugMode = true;
    public KeyCode ProgressKey = KeyCode.Space;
    public KeyCode RestartKey = KeyCode.LeftControl;

    void Update () {
        //Used to go through scenes by pressing set debug key.
        if (DebugMode && Input.GetKeyDown(ProgressKey))
            StartCoroutine(LoadSceneAsync.instance.LoadSceneAsyncByIndex((SceneManager.GetActiveScene().buildIndex + 1) % (SceneManager.sceneCountInBuildSettings - 1), true));

        if (DebugMode && Input.GetKeyDown(RestartKey))
            StartCoroutine(LoadSceneAsync.instance.LoadSceneAsyncByIndex(SceneManager.GetActiveScene().buildIndex, true));
    }
}
