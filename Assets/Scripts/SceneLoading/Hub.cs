using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Hub : MonoBehaviour {

    public void LoadLevel(int levelIndex)
    {
        Destroy(Camera.main.gameObject);
        SceneManager.LoadScene(levelIndex);
    }
}
