using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

public class HealthSpherePickup : GlobalEventListener
{
    public int healthPickupAmount = 10;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BoltEntity>().IsOwner && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControllAndData>().ApplySceneLocalHealth(healthPickupAmount);

            var removeHealthPickup = RemoveHealthPickup.Create();
            removeHealthPickup.Send();
        }
    }

    public override void OnEvent(RemoveHealthPickup evnt)
    {
        gameObject.SetActive(false);
    }

}
