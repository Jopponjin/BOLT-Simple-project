using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ZoneEventCaller : GlobalEventListener
{
    SpawnCubesEvent spawnCubesEvent;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawnCubesEvent = SpawnCubesEvent.Create();
            spawnCubesEvent.Send();
        }
    }
}
