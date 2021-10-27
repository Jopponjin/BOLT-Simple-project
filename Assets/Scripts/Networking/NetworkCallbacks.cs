using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;
using UnityEngine.SceneManagement;

/*
        
NOTE:   This will be the session main networkcallback scrip running only on the server.
        Handels any calls from clients to clients and take care of buffering event calls for connecting clients.
*/

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkCallbacks : GlobalEventListener
{
    GameObject enemyPrefab;

    public List<Photon.Bolt.Event> eventBuffer = new List<Photon.Bolt.Event>();

    public override void BoltStartBegin()
    {
        enemyPrefab = Resources.Load("Prefabs/SphereEnemy") as GameObject;
    }


    /// ---------------------------------------- Events ---------------------------------------------------------------------- ///

    // An expextion here is that spawning happend only on the server.
    public override void OnEvent(SpawnEnemy evnt)
    {
        BoltLog.Warn("SpawnEnemy EVENT called!");
        
        if (evnt.CurrentTriggerState == true)
        {
            for (int i = 0; i < evnt.SpawnAmount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(10, 10), 2, Random.Range(-10, 10));
                BoltNetwork.Instantiate(enemyPrefab, null, spawnPosition, Quaternion.identity);
            }
        }
    }


    //Every event looks pretty similar to this one, what we pass with the event depends one what it has defined in bolt assets or whats needed.
    public override void OnEvent(DamgeEvent evnt)
    {
        //This one is also good example of getting a event call from a client the needs to get pass one to the right client or NPC.
        BoltLog.Warn("DamgeEvent EVENT called!");

        var applyDamgeEvent = ApplyDamgeEvent.Create(evnt.DamgedEntity);
        applyDamgeEvent.Damge = evnt.DamgeValue;
        applyDamgeEvent.Send();
    }
    
    //Typical event that need to be buffer so the spawner in the scene cant be triggered again.
    public override void OnEvent(DisableSpawnsEvent evnt)
    {
        AddEventChangeToList(evnt);
    }

    public override void OnEvent(RemoveHealthPickup evnt)
    {
        AddEventChangeToList(evnt);
    }

    public override void OnEvent(HealthPickupEvent evnt)
    {
        //WhateverEvent.Post() is just a simplefied version of.Create() -
        //and given that RemoveHealthPickup event doesn't need anything passed on can be called right away.
        RemoveHealthPickup.Post();
    }

    /// ------------------------------------------- Synchronized Event & Scene changes ------------------------------------------ ///


    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
    {

        if (eventBuffer.Count != 0)
        {
            BoltLog.Warn("eventBuffer is NOT empty.");

            foreach (var gameEvent in eventBuffer)
            {
                if (gameEvent == DisableSpawnsEvent.Create())
                {
                    //This gets sent to all global event listeners on the specified event.
                    gameEvent.Send();
                    BoltLog.Warn(gameEvent + " was detected!");
                }
                //If there is more event cases that need to be synced include them in the same manner as previous if cases.
                else if (gameEvent == RemoveHealthPickup.Create())
                {
                    gameEvent.Send();
                }
            }
        }
    }


    /// ---------------------------------------- Server Network Buffer ------------------------------------------------------- ///

    //This make sure the server saves events that need to be synced to connecting players, This is because their local scene is not up to date.
    //In summery just a list that gets allocated when you attach the method and pass the event.

    void AddEventChangeToList(Photon.Bolt.Event calledEvent)
    {
        //Logic here is to just add a new event if there is none, otherwise just override the old with the updated one.

        if (eventBuffer.Contains(calledEvent))
        {
            int index = eventBuffer.IndexOf(calledEvent);
            eventBuffer[index] = calledEvent;

            BoltLog.Warn(calledEvent + " is updated!");

        }
        else if (!eventBuffer.Contains(calledEvent))
        {
            eventBuffer.Add(calledEvent);

            BoltLog.Warn(calledEvent + " added to eventBuffer");
        }
    }
}
