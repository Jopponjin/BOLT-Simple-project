using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] int playerDamge = 50;

    // Update is called once per frame
    void Update()
    {
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
