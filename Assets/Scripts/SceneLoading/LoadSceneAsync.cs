using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour
{
    public static LoadSceneAsync instance;

    private AsyncOperation loadLoadingScreen;

    private AsyncOperation loadLevel;
    private bool allowedSceneActivation;

    private Scene loadingScene;
    private int currentLoadingIndex;

    public bool isFading;
    public float fadeTime = 0.1f;

    private void Awake()
    {
        if (instance == null)
        {
            //Making singleton and keeping it.
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }


    //Delegate to show subscribers when the loading is done.
    private bool isFinishedLoading;
    public delegate void FinishedLoading();
    public FinishedLoading onFinishedLoading;

    public IEnumerator LoadSceneAsyncByIndex(int levelID, bool loadAutomatically = true)
    {
        if (isFading)
            yield return FadeOut();

        if (loadLoadingScreen == null)
        {
            currentLoadingIndex = levelID;

            //Rememebering scene wehave active now.
            Scene currentScene = SceneManager.GetActiveScene();

            //Creating a load operation.
            loadLoadingScreen = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

            //Stopping automatic transition.
            loadLoadingScreen.allowSceneActivation = false;

            while (!loadLoadingScreen.isDone)
            {
                if (loadLoadingScreen.progress == 0.9f)
                {
                    FindObjectOfType<AudioListener>().enabled = false;
                    loadLoadingScreen.allowSceneActivation = true;
                }
                yield return null;
            }

            FindObjectOfType<AudioListener>().enabled = false;

            loadingScene = SceneManager.GetSceneByName("LoadingScene");
            SceneManager.SetActiveScene(loadingScene);

            yield return new WaitForSeconds(0.1f);

            if (loadLevel == null)
            {
                //Creating a load operation.
                loadLevel = SceneManager.LoadSceneAsync(levelID, LoadSceneMode.Additive);
                loadLevel.completed += LoadSceneCompleted;

                //Stopping automatic transition.
                loadLevel.allowSceneActivation = false;

                allowedSceneActivation = loadAutomatically;

                bool isUnloading = false;

                while (loadLevel != null && !loadLevel.isDone)
                {
                    //Sending a message to sunscribers that the loading is done.
                    if (loadLevel.progress == 0.9f)
                    {
                        if (allowedSceneActivation)
                        {
                            loadLevel.allowSceneActivation = true;

                            if (!isUnloading)
                            {
                                SceneManager.UnloadSceneAsync(currentScene);
                                isUnloading = true;
                            }
                        }
                    }
                    yield return null;
                }
            }
        }
        if(isFading)
            StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        GameObject fadeObject = GameObject.Find("FadeCanvas/FadeImage");

        if (fadeObject)
        {
            UnityEngine.UI.Image fadeImage = fadeObject.GetComponent<UnityEngine.UI.Image>();
            fadeImage.GetComponent<CanvasRenderer>().SetAlpha(0);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            fadeImage.CrossFadeAlpha(1, fadeTime, false);
        }

        yield return new WaitForSeconds(fadeTime);
    }


    private IEnumerator FadeIn()
    {
        GameObject fadeObject = GameObject.Find("FadeCanvas/FadeImage");

        if (fadeObject)
        {
            UnityEngine.UI.Image fadeImage = fadeObject.GetComponent<UnityEngine.UI.Image>();
            fadeImage.GetComponent<CanvasRenderer>().SetAlpha(1);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            fadeImage.CrossFadeAlpha(0, fadeTime, false);
        }

        yield return new WaitForSeconds(fadeTime);
    }

    /// <summary>
    /// Method that gets called when the scene is loaded.
    /// </summary>
    /// <param name="obj"></param>
    private void LoadSceneCompleted(AsyncOperation sceneLoad)
    {
        FindObjectOfType<AudioListener>().enabled = false;
        loadLevel = null;

        UnloadLoadScene();
    }

    private void UnloadLoadScene()
    {
        Scene loadedScene = SceneManager.GetSceneByBuildIndex(currentLoadingIndex);
        SceneManager.SetActiveScene(loadedScene);
        SceneManager.UnloadSceneAsync(loadingScene);

        loadLoadingScreen = null;
    }

    /// <summary>
    /// Method used for activating the transitioning of scenes.
    /// </summary>
    public void AllowSceneActivation()
    {
        if (loadLevel != null)
            allowedSceneActivation = true;
    }
}