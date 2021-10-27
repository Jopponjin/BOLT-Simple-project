using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

public class HealthSpherePickup : GlobalEventListener
{
    public int healthPickupAmount = 10;

    // Event get called when the local player enters trigger health get applied and a event gets sent.
    private void OnTriggerEnter(Collider other)
    {
        //Logic for making sure only the local player can call this.
        if (other.GetComponent<BoltEntity>().IsOwner && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControllAndData>().ApplyHealth(healthPickupAmount);

            var removeHealthPickup = RemoveHealthPickup.Create();
            removeHealthPickup.Send();
        }
    }

    // Gets called if an player enter a trigger disabling it.
    public override void OnEvent(RemoveHealthPickup evnt)
    {
        gameObject.SetActive(false);
    }

}
