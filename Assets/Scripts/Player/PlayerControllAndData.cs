using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class PlayerControllAndData : EntityBehaviour<ICustomPlayer>
{
    public CharacterController characterController;

    Vector3 moveDirection;
    float moveX;
    float moveZ;
    public float movementSpeed = 2f;
    public int localHealth = 100;
    public string playerUsername;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, gameObject.transform);

        if (entity.IsOwner)
        {
            state.Color = new Color(Random.value, Random.value, Random.value);
            state.Health = localHealth;
        }

        state.AddCallback("Color", ColorChanged);
        state.AddCallback("Health", HealthCallback);

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
        Debug.Log("ApplyDamge called: damge = " +damgeValue);
        state.Health -= damgeValue;
    }

    void HealthCallback()
    {
        Debug.Log("HealthCallback called!");
        localHealth = state.Health;
    }
}
