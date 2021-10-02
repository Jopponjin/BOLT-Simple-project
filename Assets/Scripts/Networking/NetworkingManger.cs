using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;
using System;
using Photon.Realtime;

public class NetworkingManger : GlobalEventListener
{
    public static NetworkingManger instance;
    UdpSession photonSession;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void JoinServer()
    {
        var auth = new AuthenticationValues();
        auth.UserId = BoltNetwork.UdpSocket.WanEndPoint.Address.ToString();
        BoltLauncher.StartClient();
    }

    public void StartServer()
    {
        BoltLauncher.StartServer();
    }

    public void LeaveServer()
    {
        BoltLauncher.Shutdown();
        Application.Quit();
    }

    public void BoltSpawnPlayer()
    {
        if (BoltNetwork.IsClient)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 2f, UnityEngine.Random.Range(10, -10));
            var spawnedPlayer = BoltNetwork.Instantiate(GameData.instance.playerPrefab);
            spawnedPlayer.transform.position = spawnPosition;

            spawnedPlayer.TakeControl();
        }
        else if (BoltNetwork.IsServer)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), 2f, UnityEngine.Random.Range(10, -10));
            var spawnedPlayer = BoltNetwork.Instantiate(GameData.instance.playerPrefab);
            spawnedPlayer.transform.position = spawnPosition;
            spawnedPlayer.TakeControl();
        }
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = "Test Server";

            BoltMatchmaking.CreateSession(sessionID: matchName, sceneToLoad: "Level1");

            var startEvent = ServerStarted.Create();
            startEvent.Send();
        }
    }

    public override void SessionConnected(UdpSession session, IProtocolToken token)
    {
        
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
                BoltMatchmaking.JoinSession(photonSession);
            }
        }
    }

}
