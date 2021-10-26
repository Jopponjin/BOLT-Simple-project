using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;
using Photon.Bolt.Utils;

public class PlayerControllAndData : EntityEventListener<ICustomPlayer>
{
    public CharacterController characterController;

    public BoltEntity character;
    public BoltConnection connection;
    

    Vector3 moveDirection;
    float moveX;
    float moveZ;

    public float movementSpeed = 2f;
    public int localHealth = 100;
    public string playerUsername;

    //Here we just attach callback to values that need to be synced across the server/bolt network.
    public override void Attached()
    {
        state.SetTransforms(state.Transform, gameObject.transform);

        if (GetComponent<BoltEntity>().IsOwner)
        {
            state.Color = new Color(Random.value, Random.value, Random.value);
            state.Health = localHealth;
        }

        state.AddCallback("Color", ColorChanged);

        characterController = GetComponent<CharacterController>();
    }

    // Controlls put here in order to simlefie input controll on the entitys that are controlled,
    // there might be ways to input controll with out useing bolts own update function if thats needed.
    public override void SimulateController()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        moveDirection += new Vector3(moveX, 0, moveZ) * BoltNetwork.FrameDeltaTime;
        gameObject.transform.position = moveDirection * movementSpeed;
    }

    /// --------------------------------------------- Events --------------------------------------------- ///


    public override void OnEvent(ApplyDamgeEvent evnt)
    {
        BoltLog.Warn("ApplyDamgeEvent EVENT called!");
        ApplyDamge(evnt.Damge);
    }

    /// --------------------------------------------- Player Modfiers ----------------------------------- ///


    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.Color;
    }

    /// --------------------------------------- Scene Local -------------------------------------------- ///

    public void ApplyDamge(int damgeValue)
    {
        BoltLog.Warn("ApplyDamge called: damge - = " + damgeValue);
        if (gameObject.GetComponent<BoltEntity>().IsOwner && state.Health > 0)
        {
            state.Health -= damgeValue;
            localHealth = state.Health;
        }   
    }

    public void ApplyHealth(int healthValue)
    {
        BoltLog.Warn("ApplyHealth called: health + = " + healthValue);
        if (gameObject.GetComponent<BoltEntity>().IsOwner)
        {
            if (state.Health <= 100)
            {
                if (state.Health + healthValue > 100)
                {
                    BoltLog.Warn("ApplyHealth called: Given health would be greater than max, diffrance given :" + healthValue);
                    state.Health += (100 - state.Health);
                    localHealth = state.Health;
                }
                else
                {
                    state.Health += healthValue;
                    localHealth = state.Health;
                }
            }
        }
    }

    
}
