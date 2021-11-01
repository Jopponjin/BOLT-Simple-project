using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    Button joinButton;
    Button hostButton;
    Button quitButton;

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
    }

    public void JoinServer()
    {
        NetworkingManger.instance.JoinServer();
    }

    public void HostServer()
    {
        NetworkingManger.instance.StartServer();
    }

    public void Quit()
    {
        CoreGameState.instance.QuitGameApplication();
    }
}
