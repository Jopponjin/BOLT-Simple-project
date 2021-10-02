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
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel()
    {
        if (GameData.instance.currentScene.name == "Level1")
        {
            //Load Menu scene
            SceneManager.LoadSceneAsync(SceneManager.GetSceneByName("Menu").buildIndex);
        }
    }

    public void QuitGameApplication()
    {
        Application.Quit();
    }
}
