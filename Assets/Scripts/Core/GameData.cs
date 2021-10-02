using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public Scene currentScene;
    [Space]
    public GameObject playerPrefab;
}
