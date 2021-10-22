using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;
/*

NOTE:   Networking for this part is just an call for the NetworkCallback on the server
        which in turn send another event to all other clients if this object in order to disable any duplicate calls.


*/
public class ZoneEventCaller : GlobalEventListener
{
    public bool callOnce = true;
    Material material;
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
                var spawnEnemyEvent = SpawnEnemy.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                spawnEnemyEvent.CurrentTriggerState = callOnce;
                spawnEnemyEvent.SpawnAmount = 5;
                spawnEnemyEvent.EntityCaller = GetComponent<BoltEntity>();
                spawnEnemyEvent.Send();

                gameObject.GetComponent<Renderer>().material.color = Color.magenta;

                //Deppending on if it should be call once we need to releay its to the server callback.
                if (callOnce)
                {
                    triggerState = TriggerState.Disabled;

                    var triggerDisabledEvent = DisableSpawnsEvent.Create(GlobalTargets.Everyone, ReliabilityModes.ReliableOrdered);
                    triggerDisabledEvent.DisabledColor = Color.magenta;
                    triggerDisabledEvent.CallerEntity = GetComponent<BoltEntity>();
                    triggerDisabledEvent.Send();
                }
            }
        }
    }


    //This gets called by the server in order to sync the state of this object on each client.
    public override void OnEvent(DisableSpawnsEvent evnt)
    {
        BoltLog.Warn("CallbackDisableTrigger Called, " + gameObject.name + " triggger is Disabled!");

        triggerState = TriggerState.Disabled;
        GetComponent<Renderer>().material.color = Color.magenta;
    }

}
