using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkEventCallbacks : GlobalEventListener
{

    public override void BoltStartBegin()
    {
        //BoltNetwork.RegisterTokenClass<NetworkClientData>();
    }

    public override void OnEvent(DamgeEvent evnt)
    {
        var playerEntity = BoltNetwork.FindEntity(evnt.NetworkId);

        if (playerEntity != null)
        {
            Debug.Log("called damge event on " + playerEntity.name);
            playerEntity.GetComponent<PlayerControllAndData>().ApplyDamge(evnt.eventDamgeValue);
        }
    }
}

