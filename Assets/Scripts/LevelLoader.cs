using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int iLevelToLoad;
    public string sLevelToLoad;

    public bool useIntegerToLoadLevel = false;

    public bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.CompareTag("Player"))
            LoadScene();
    }

    public void LoadScene()
    {
        if (useIntegerToLoadLevel)
            SceneManager.LoadScene(iLevelToLoad);
        else
            SceneManager.LoadScene(sLevelToLoad);

    }

    public void Exit() {
        Application.Quit();
    }
}
