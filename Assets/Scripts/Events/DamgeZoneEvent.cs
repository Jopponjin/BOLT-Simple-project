using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;


public class DamgeZoneEvent : GlobalEventListener
{
    public int zoneDamgeAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<BoltEntity>().IsOwner)
        {
            //DamgeEvent damgeEvent = DamgeEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered);
            //damgeEvent.PlayerId = other.GetComponent<BoltEntity>().PrefabId;
            //damgeEvent.NetworkId = other.GetComponent<BoltEntity>().NetworkId;
            //damgeEvent.eventDamgeValue = zoneDamgeAmount;
            //damgeEvent.Send();

            other.GetComponent<PlayerControllAndData>().ApplyDamge(zoneDamgeAmount);
            
        }
    }
}
