using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;


public class DamgeZoneEvent : EntityBehaviour<ICustomPlayer>
{
    public int zoneDamgeAmount = 10;

    //Dosent need an event to apply damge on the client just check for 'isOwner'.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<BoltEntity>().IsOwner)
        {
            other.gameObject.GetComponent<PlayerControllAndData>().ApplyDamge(zoneDamgeAmount);
        }
    }
}
