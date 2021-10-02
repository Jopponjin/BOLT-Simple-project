using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using System;
using UdpKit;

public class NetworkClientData : MonoBehaviour
{
    public int playerId;
    public int playerIP;

    public void Read(UdpPacket packet)
    {
        packet.WriteInt(playerId);
        packet.WriteInt(playerIP);
    }

    public void Write(UdpPacket packet)
    {
        playerId = packet.ReadInt();
        playerIP = packet.ReadInt();
    }
}
