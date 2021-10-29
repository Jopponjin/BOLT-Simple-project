using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using System;
using Photon.Realtime;
using Photon.Bolt.Utils;

/*
NOTE:   This is a local network manger its controls hosting, joining and some miscellaneous data.
        This only run locally on the clients and doesn't interact a lot with the server expect when joining, disconnecting or spawning a player.
        Most methods here are self explanatory and are pretty encapsulated in terms of function.
*/
public class NetworkingManger : GlobalEventListener
{
    public static NetworkingManger instance;

    public MenuUI menuUI;
    public Dictionary<BoltConnection, BoltEntity> connectionList = new Dictionary<BoltConnection, BoltEntity>();
    
    UdpSession photonSession;

    public GameObject playerPrefab;

    // We make the class a singleton as the functions of joining and disconnecting etc. are needed during gameplay.
    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else instance = this;

        DontDestroyOnLoad(this);
        instance = this;
    }

    // This gets called when we host a game on the main menu.
    // Simply defines name for the session an creates a session with the selected scene if you are a server.
    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = "Test Server";

            BoltMatchmaking.CreateSession(sessionID: matchName, null, sceneToLoad: "Level1");
        }
    }

    //Call to join a server, what server? it gets define further down in the script.
    public void JoinServer()
    {
        BoltLauncher.StartClient();
    }

    //Start making a session to host, you can refere to BoltStartDone().
    public void StartServer()
    {
        BoltLauncher.StartServer();
    }


    public void LeaveServer()
    {
        if (BoltNetwork.IsConnected)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            BoltLauncher.Shutdown();
        }
        else if (BoltNetwork.IsServer)
        {
            //There need to be some more logic added to disconnect player before the server closes or you have to make a host migration solution.
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            BoltLauncher.Shutdown();
        }
    }

    //This spawn function is called manually after the new scene is done loading and you can press the spawn button in the ui.
    // Their is some checks to see if your a client or the host and apply some code.
    public void BoltSpawnPlayer()
    {
        if (BoltNetwork.IsClient)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 2f, UnityEngine.Random.Range(10, -10));
            var spawnedPlayer = BoltNetwork.Instantiate(playerPrefab);
            spawnedPlayer.transform.position = spawnPosition;

            spawnedPlayer.TakeControl();

            var spawnEntityEvent = PlayerPrefabMade.Create();
            spawnEntityEvent.PlayerEntityPrefab = spawnedPlayer.PrefabId;
            spawnEntityEvent.PlayerNetworkId = spawnedPlayer.NetworkId;
            spawnEntityEvent.Send();
            

            Debug.LogWarning("PlayerPrefabMade Event Raised");
            return;
        }
        else if (BoltNetwork.IsServer)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 2f, UnityEngine.Random.Range(10, -10));
            var spawnedPlayer = BoltNetwork.Instantiate(playerPrefab);
            spawnedPlayer.transform.position = spawnPosition;
            spawnedPlayer.TakeControl();
        }
    }

    //This acts as player list which is a bit easier than try to get the same information from Bolt.
    public override void Connected(BoltConnection connection)
    {
        if (BoltNetwork.IsServer)
        {
            connectionList.Add(connection, null);
        }
        else if (BoltNetwork.IsClient)
        {
            Debug.LogWarning(connection.ConnectionId.ToString() + " is connected");
        }
    }

    // The method can bes used to get some info before a client join, can be use for stuff like getting nicknames or checking the password they wrote before joining.
    public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken userToken)
    {

        BoltNetwork.Accept(endpoint, null);

    }

    //This gets called after the client start their bolt instance and bolt requests a list of sessions.
    //So this only a list you can go through and find a session according to what we are looking for, in this case just a open session.
    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);

        foreach (var session in sessionList)
        {
            photonSession = session.Value;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                JoinServer(photonSession);
            }
        }
    }

    // Here we join the saved session we got earlier.
    void JoinServer(UdpSession photonSession)
    {
        BoltMatchmaking.JoinSession(photonSession, null);
    }

    //Simple disconnect call which the server remove a client from the connection list,
    public override void Disconnected(BoltConnection connection)
    {
        if (BoltNetwork.IsServer)
        {
            connectionList.Remove(connection);
            connectionList.StripKeysWithNullValues();
        }
    }

}
