using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using UdpKit;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkingRuntimeData : GlobalEventListener
{
    public static NetworkingRuntimeData instance;

    public BoltConnection clientIp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public override void Connected(BoltConnection connection)
    {
        
        
    }

    public override void Disconnected(BoltConnection connection)
    {
        
    }

    public BoltConnection ReturnClientConnection(uint connectionId)
    {
        
        return null;
    }
}
