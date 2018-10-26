using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour {

    /// <summary>
    /// Method for proceeding to the game.
    /// </summary>
	public void ProceedToGame()
    {
        //Start the loading of the character selection screen.
        StartCoroutine(LoadSceneAsync.instance.LoadSceneAsyncByIndex((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings, true));
    }
}
