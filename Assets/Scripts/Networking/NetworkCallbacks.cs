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

    public override void OnEvent(DamgeEvent evnt)
    {
        BoltLog.Warn("DamgeEvent EVENT called!");

        var applyDamgeEvent = ApplyDamgeEvent.Create(evnt.DamgedEntity);
        applyDamgeEvent.Damge = evnt.DamgeValue;
        applyDamgeEvent.Send();
    }
    
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
        RemoveHealthPickup.Post();
    }

    /// ------------------------------------------- Syncronized Event&Scene changes ------------------------------------------ ///


    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
    {

        if (eventBuffer.Count != 0)
        {
            BoltLog.Warn("eventBuffer is NOT empty.");

            foreach (var gameEvent in eventBuffer)
            {
                if (gameEvent == DisableSpawnsEvent.Create())
                {
                    //This gets sent to all listners on the specefid event.
                    gameEvent.Send();
                    BoltLog.Warn(gameEvent + " was detected!");
                }
                //If there is more event cases that need to be synced include them in the same manner sa previous if cases.
                else if (gameEvent == RemoveHealthPickup.Create())
                {
                    gameEvent.Send();
                }
            }
        }
    }


    /// ---------------------------------------- Server Network Buffer ------------------------------------------------------- ///

    //This make sure the server saves events that need to be synced to connecting players as there local scene is not up to date.
    //In summery just a list that gets alocated when you attatch the method in event method and pass the event.

    void AddEventChangeToList(Photon.Bolt.Event calledEvent)
    {
        //Logic here is to just add a new event if there is none otherwise just overide the old with the latest one.

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
