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

    // Controls put here in order to simplify input control on the entity as the would need to add additional logic.
    // there might be ways to input control with out using bolts own update function if thats needed.
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

    /// --------------------------------------------- Player Modifiers ----------------------------------- ///


    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.Color;
    }

    /// --------------------------------------- Scene Local -------------------------------------------- ///
    //Both ApplyDamge and ApplyHealth work when checking if it the owner and that the state health isn't to low or high to apply either.

    public void ApplyDamge(int damgeValue)
    {
        BoltLog.Warn("ApplyDamge called: damage - = " + damgeValue);
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
                    BoltLog.Warn("ApplyHealth called: Given health would be greater than max, difference given :" + healthValue);
                    state.Health += (100 - state.Health);
                    localHealth = state.Health;
                }
                else
                {
                    // LocalHealth is only here for debugging reasons.
                    state.Health += healthValue;
                    localHealth = state.Health;
                }
            }
        }
    }

    
}
