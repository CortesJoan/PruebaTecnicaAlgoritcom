using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRigidbody;

    [Header("Config")] 
    [SerializeField] private float moveSpeed;
    public Transform moveRelativeTo;

    [Header("Debug")] 
    [SerializeField] private Vector3 currentMoveDirection;
    private bool canMove = true;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
    private const float minimumVelocityMagnitudeToStop = 0.1f;

    [Header("Events")]
    [SerializeField] private UnityEvent onMoveStopped;
    [SerializeField] private UnityEvent onMoveStarted;


    public void ChangeMoveDirection(Vector3 newMoveDirection)
    {
        currentMoveDirection = newMoveDirection;
    }

    public void ChangeMoveSpeed(float newSpeed)
    {
        this.moveSpeed = newSpeed;
    }

    private void Update()
    {
        if (currentMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentMoveDirection);
        }
    }

    public void FixedUpdate()
    {
        previousVelocity = playerRigidbody.velocity;

        currentVelocity = currentMoveDirection * (moveSpeed * Time.fixedDeltaTime);
        playerRigidbody.velocity = currentVelocity;
        FireMoveEvents();
    }

    void FireMoveEvents()
    {
        if (previousVelocity.magnitude > minimumVelocityMagnitudeToStop &&
            currentVelocity.magnitude < minimumVelocityMagnitudeToStop)
        {
            onMoveStopped?.Invoke();
        }
        else if (previousVelocity.magnitude < minimumVelocityMagnitudeToStop &&
                 currentVelocity.magnitude > minimumVelocityMagnitudeToStop)
        {
            onMoveStarted?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }
        Vector2 moveDirection = context.ReadValue<Vector2>();
   
        if (moveRelativeTo == null)
        {
            moveRelativeTo = this.transform;
        }
        currentMoveDirection =
            moveRelativeTo.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.y));
        currentMoveDirection.y = 0;
        if (moveDirection.normalized == new Vector2(0.71f, 0.71f).normalized)
        {
            Debug.Log("true");
        }
        Debug.Log("Move direction " + moveDirection);
    }

    public void BlockMovement()
    {
        currentMoveDirection = Vector3.zero;
        canMove = false;
    }

    public void UnblockMovement()
    {
        canMove = true;
    }

    public Transform GetMoveRelativeTo()
    {
        return moveRelativeTo;
    }
    
    public void SetMoveRelativeTo(Transform newMoveRelativeTo)
    {
        moveRelativeTo = newMoveRelativeTo;
    }
}