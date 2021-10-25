using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Bolt;

public class PauseMenuUI : EntityBehaviour<ICustomPlayer>
{
    GameObject pauseMenu;
    
    Button continueButton;
    Button quitButton;

    bool isPaused;

    private void Start()
    {
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            if (button.name == "ContinueButton")
            {
                continueButton = gameObject.GetComponentInChildren<Button>();
            }
            if (button.name == "QuitButton")
            {
                quitButton = gameObject.GetComponentInChildren<Button>();
            }
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && !isPaused)
        {
            TogglePauseMenu(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && isPaused)
        {
            TogglePauseMenu(false);
        }
    }

    public void TogglePauseMenu(bool pasueMenuState)
    {
        if (pasueMenuState)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            isPaused = true;
        }
        else if (!pasueMenuState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseMenu.SetActive(false);
            isPaused = false;
        }
    }

    public void SpawnPlayer()
    {
        NetworkingManger.instance.BoltSpawnPlayer();
    }

    public void QuitGame()
    {
        NetworkingManger.instance.LeaveServer();
        CoreGameState.instance.ChangeLevel();
    }
}
