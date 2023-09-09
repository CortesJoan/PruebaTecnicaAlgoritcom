using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    
    [SerializeField] private Rigidbody rigidbody;

    [SerializeField] private Vector3 currentMoveDirection;
    [SerializeField] private float moveForce;


    [SerializeField] private Vector3 currentVelocity;

    public void ChangeMoveDirection(Vector3 newMoveDirection)
    {
        currentMoveDirection = newMoveDirection;
    }


    public void FixedUpdate()
    {
         rigidbody.velocity = transform.TransformDirection(currentMoveDirection) * (moveForce * Time.fixedDeltaTime);
        currentVelocity = rigidbody.velocity;
    }



    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection=        context.ReadValue<Vector2>();
        currentMoveDirection = new Vector3(moveDirection.x,currentMoveDirection.y,moveDirection.y);

    }
}