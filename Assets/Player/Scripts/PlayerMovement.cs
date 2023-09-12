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
        rigidbody.velocity = currentMoveDirection * (moveForce * Time.fixedDeltaTime);
        //      rigidbody.velocity = transform.TransformDirection(currentMoveDirection) * (moveForce * Time.fixedDeltaTime);
        currentVelocity = rigidbody.velocity;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>();
        // move direction possible values 
        // 1,0    X right  y rotation of 90
        //-1,0    X left y rotation of -90
        //0,1    Z forward y rotation of -180 
        //0,-1    Z backward  y rotation of 180


        currentMoveDirection = new Vector3(moveDirection.x, currentMoveDirection.y, moveDirection.y);
        if (moveDirection.normalized == new Vector2(0.71f, 0.71f).normalized)
        {
            Debug.Log("true");
        }
        Debug.Log("Move direction " + moveDirection);
        if (moveDirection != Vector2.zero)
        {
            // transform.localRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, transform.localRotation.y, moveDirection.y).normalized);
            float angle = Mathf.Atan2(moveDirection.normalized.x, moveDirection.normalized.y);
            angle *= Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, angle, 0);  
        }
    }
}