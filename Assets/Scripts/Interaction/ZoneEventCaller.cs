using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

public class ZoneEventCaller : GlobalEventListener
{
    public bool callOnce = true;

    public enum TriggerState
    {
        Disabled,
        Active
    }
    TriggerState triggerState = TriggerState.Active;

    //For tirgger objects that are local only need to be check that only clients that are local can interact with them.
    //Otherwise the event might be called on all clients when you can call their trigger when there should be one call.
    private void OnTriggerEnter(Collider other)
    {
        if (triggerState == TriggerState.Active)
        {
            if (other.gameObject.CompareTag("Player") && other.GetComponent<BoltEntity>().IsOwner)
            {
                var spawnEnemyEvent = SpawnEnemy.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered);
                spawnEnemyEvent.EntityCaller = other.GetComponent<PlayerControllAndData>().entity;
                spawnEnemyEvent.NetworkId = other.GetComponent<BoltEntity>().NetworkId;
                spawnEnemyEvent.PrefabId = other.GetComponent<BoltEntity>().PrefabId;
                spawnEnemyEvent.CurrentTriggerState = callOnce;
                spawnEnemyEvent.SpawnAmount = 5;
                spawnEnemyEvent.Send();

                if (callOnce)
                {
                    triggerState = TriggerState.Disabled;

                    var triggerDisabledEvent = DisableSpawnsEvent.Create(GlobalTargets.OnlyServer);
                    triggerDisabledEvent.disabledTriggerObject = gameObject.transform.rotation;
                    triggerDisabledEvent.Send();
                }
            }
        }
    }



    public override void OnEvent(CallbackDisableTrigger evnt)
    {
        BoltLog.Warn("CallbackDisableTrigger Called, " + gameObject.name + " triggger is Disabled!");
        triggerState = TriggerState.Disabled;
    }


}
