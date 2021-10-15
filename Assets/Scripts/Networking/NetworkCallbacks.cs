using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkCallbacks : GlobalEventListener
{
    private GameObject enemyPrefab;

    public override void BoltStartBegin()
    {
        enemyPrefab = Resources.Load("Prefabs/SphereEnemy") as GameObject;

        BoltLog.Warn(enemyPrefab.name + "is being loaded");
    }

    public override void OnEvent(ServerStarted evnt)
    {
        
        BoltLog.Warn("ServerStarted EVENT called!");
    }

    public override void OnEvent(PlayerPrefabMade evnt)
    {
        
        BoltLog.Warn("PlayerPrefabMade EVENT called!");
    }

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

    }

    public override void OnEvent(DisableSpawnsEvent evnt)
    {
        var callbackDisableTrigger = CallbackDisableTrigger.Create(GlobalTargets.Everyone);
        callbackDisableTrigger.Send();
    }
}
