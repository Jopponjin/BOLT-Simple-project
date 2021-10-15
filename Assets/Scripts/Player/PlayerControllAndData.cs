using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerControllAndData : EntityEventListener<ICustomPlayer>
{
    public CharacterController characterController;

    public BoltEntity character;
    public BoltConnection connection;

    public bool IsServer
    {
        get { return connection == null; }
    }
    public bool IsClient
    {
        get { return connection != null; }
    }

    Vector3 moveDirection;
    float moveX;
    float moveZ;

    public float movementSpeed = 2f;
    public int localHealth = 100;
    public string playerUsername;

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

    public override void SimulateController()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");

        moveDirection += new Vector3(moveX, 0, moveZ) * BoltNetwork.FrameDeltaTime;
        gameObject.transform.position = moveDirection * movementSpeed;
    }

    void ColorChanged()
    {
        GetComponent<Renderer>().material.color = state.Color;
    }

    public void ApplyDamge(int damgeValue)
    {
        Debug.Log("ApplyDamge called: damge = " + damgeValue);
        if (gameObject.GetComponent<BoltEntity>().IsOwner)
        {
            localHealth -= damgeValue;
        }
        
    }

    
}
