using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoreGameState : MonoBehaviour
{
    public static CoreGameState instance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;

        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void ChangeLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            //Load Menu scene
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void QuitGameApplication()
    {
        Application.Quit();
    }
}
