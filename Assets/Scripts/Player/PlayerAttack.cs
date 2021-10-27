using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] int playerDamge = 50;

    
    void Update()
    {
        //Attack code is just a event call with the entity refrance passed through of the one we damged.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player && player != gameObject)
            {
                var damgeEvent = DamgeEvent.Create();
                damgeEvent.DamgeValue = playerDamge;
                damgeEvent.DamgedEntity = player.GetComponent<BoltEntity>();
                damgeEvent.Send();
            }
        }
    }

    //Just  simple logic to make sure we can damage the entity.
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        player = null;
    }
}
