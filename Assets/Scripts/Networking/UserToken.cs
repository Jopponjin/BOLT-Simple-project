using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using System;
using UdpKit;
using Photon.Bolt.Utils;

public class UserToken : MonoBehaviour, IProtocolToken
{ 
    public string sessionPassword;
    public string playerUsername;

    public void Read(UdpPacket packet)
    {
        packet.WriteString(sessionPassword);
        packet.WriteString(playerUsername);
    }

    public void Write(UdpPacket packet)
    {
        sessionPassword = packet.ReadString();
        playerUsername = packet.ReadString();
    }
}
