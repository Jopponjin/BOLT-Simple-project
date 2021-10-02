using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class SpawnEnemyCubesEvent : GlobalEventListener
{
    public GameObject objectToSpawn;
    public int spawnAmount = 10;

    public override void OnEvent(ServerStarted evnt)
    {
        objectToSpawn = Resources.Load("Prefabs/SphereEnemy")as GameObject;
        if (objectToSpawn == null)
        {
            Debug.Log("objectToSpawn is not assigned");
        }
        Debug.Log("ServerStarted event called!");
    }

    public override void OnEvent(SpawnCubesEvent evnt)
    {
        SpawnEnemys();
    }

    public void SpawnEnemys()
    {
        Debug.Log("SpawnEnemys called!");
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPostRandom = new Vector3(UnityEngine.Random.Range(10, -10), 2, UnityEngine.Random.Range(10, -10));
            BoltNetwork.Instantiate(objectToSpawn, spawnPostRandom, Quaternion.identity);
        }
    }

}
