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

public class NetworkingManger : GlobalEventListener
{
    public static NetworkingManger instance;
    PlayerControllAndData playerControllAndData;
    public MenuUI menuUI;
    public Dictionary<BoltConnection, BoltEntity> connectionList = new Dictionary<BoltConnection, BoltEntity>();
    
    UdpSession photonSession;

    public string localPlayerName;
    public string clientPlayerName;

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

    public override void BoltStartBegin()
    {
        BoltNetwork.RegisterTokenClass<UserToken>();
    }

    public void JoinServer()
    {
        BoltLauncher.StartClient();
    }

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
            SceneManager.LoadScene(0, LoadSceneMode.Single);
            BoltLauncher.Shutdown();
        }
    }

    public void BoltSpawnPlayer()
    {
        if (BoltNetwork.IsClient)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 2f, UnityEngine.Random.Range(10, -10));
            var spawnedPlayer = BoltNetwork.Instantiate(GameData.instance.playerPrefab);
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
            var spawnedPlayer = BoltNetwork.Instantiate(GameData.instance.playerPrefab);
            spawnedPlayer.transform.position = spawnPosition;
            spawnedPlayer.TakeControl();
        }
    }

    public void PassOnPlayerData(string playerName)
    {
        BoltLog.Warn("PassOnPlayerData CALLED! player name: " + playerName);

        localPlayerName = playerName;
    }

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

    public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken userToken)
    {
        UserToken userInfoToken = new UserToken();
        clientPlayerName =  userInfoToken.playerUsername.ToString();

        BoltNetwork.Accept(endpoint, userInfoToken);
        

        if (userInfoToken.playerUsername != string.Empty)
        {
            BoltLog.Warn("infoToken is NOT false!");
            Debug.LogWarning("infoToken is NOT false!");

            //Debug.LogWarning("infoToken username: " + userInfoToken.playerUsername);
            //BoltLog.Warn("infoToken username: " + userInfoToken.playerUsername);
            BoltLog.Warn("infoToken username: " + clientPlayerName);

            return;
        }
        else
        {
            Debug.LogWarning("infoToken IS false!");
            BoltLog.Warn("infoToken IS false!");
            
            BoltNetwork.Refuse(endpoint, userInfoToken);
        }

    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = "Test Server";

            UserToken userInfoToken = new UserToken();

            BoltMatchmaking.CreateSession(sessionID: matchName, userInfoToken, sceneToLoad: "Level1");
        }
    }

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        GameData.instance.currentScene = SceneManager.GetSceneByName(scene);


    }

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

    void JoinServer(UdpSession photonSession)
    {
        UserToken userToken = new UserToken();

        userToken.playerUsername = localPlayerName.ToString();

        BoltMatchmaking.JoinSession(photonSession, userToken);
    }

    public override void Disconnected(BoltConnection connection)
    {
        if (BoltNetwork.IsServer)
        {
            connectionList.Remove(connection);
            connectionList.StripKeysWithNullValues();
        }
    }

    public override void OnEvent(PlayerPrefabMade evnt)
    {
        if (connectionList.ContainsKey(evnt.RaisedBy))
        {

        }
    }

}
