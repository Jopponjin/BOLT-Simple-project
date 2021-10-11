using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    Button joinButton;
    Button hostButton;
    Button quitButton;

    public InputField playerNameField;

    void Start()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            if (button.name == "JoinButton")
            {
                joinButton = gameObject.GetComponentInChildren<Button>();
            }
            if (button.name == "HostButton")
            {
                hostButton = gameObject.GetComponentInChildren<Button>();
            }
            if (button.name == "QuitButton")
            {
                quitButton = gameObject.GetComponentInChildren<Button>();
            }
        }

        playerNameField = GetComponentInChildren<InputField>();
    }

    public void JoinServer()
    {
        NetworkingManger.instance.JoinServer();
    }

    public void HostServer()
    {
        NetworkingManger.instance.StartServer();
    }

    public void ApplyPlayerName(string nameInput)
    {

        if (playerNameField.text == string.Empty)
        {
            Debug.LogWarning("No player name");
        }
        else
        {
            Debug.LogWarning("Has Player name");
            NetworkingManger.instance.PassOnPlayerData(playerNameField.text);
        }
    }

    public void Quit()
    {
        CoreGameState.instance.QuitGameApplication();
    }
}
