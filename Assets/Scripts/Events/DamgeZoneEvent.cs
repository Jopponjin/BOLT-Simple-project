using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;


public class DamgeZoneEvent : EntityBehaviour<ICustomPlayer>
{
    public int zoneDamgeAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<BoltEntity>().IsOwner)
        {
            other.gameObject.GetComponent<PlayerControllAndData>().ApplySceneLocalDamge(zoneDamgeAmount);
        }
    }
}
