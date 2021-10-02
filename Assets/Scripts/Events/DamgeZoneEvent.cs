using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;


public class DamgeZoneEvent : EntityEventListener<ICustomPlayer>
{
    public int zoneDamgeAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var damgeEvent = DamgeEvent.Create();
            damgeEvent.PlayerId = other.GetComponent<BoltEntity>().PrefabId;
            damgeEvent.NetworkId = other.GetComponent<BoltEntity>().NetworkId;
            damgeEvent.eventDamgeValue = zoneDamgeAmount;
            damgeEvent.Send();

            //other.gameObject.GetComponent<PlayerControllAndData>().ApplyDamge(zoneDamgeAmount);
        }
    }
}
