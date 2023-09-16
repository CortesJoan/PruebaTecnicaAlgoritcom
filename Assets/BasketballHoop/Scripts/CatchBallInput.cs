using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CatchBallInput : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private string ballTag = "Ball";

    [Header("Events")]
    public UnityEvent<GameObject> onBallCaught;
 
    private GameObject ballOwnerShipHandlerGameObject;
    private bool canCatchBall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ballTag))
        {
            var ballOwnershipHandler = other.GetComponentInParent<BallOwnershipHandler>();
            if (ballOwnershipHandler)
            {
                this.ballOwnerShipHandlerGameObject = ballOwnershipHandler.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(ballTag))
        {
            this.ballOwnerShipHandlerGameObject = null;
        }
    }

    public void CatchBallIfDetected(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || !ballOwnerShipHandlerGameObject)
        {
            return;
        }
        CatchBallWithOwnership(ballOwnerShipHandlerGameObject.GetComponent<BallOwnershipHandler>());
    }

    void CatchBallWithOwnership(BallOwnershipHandler ballOwnershipHandler)
    {
        if (!ballOwnershipHandler || ballOwnershipHandler.HasOwner())
        {
            return;
        }
        ballOwnerShipHandlerGameObject = ballOwnershipHandler.gameObject;
        onBallCaught?.Invoke(ballOwnerShipHandlerGameObject);
    }

 

    public GameObject GetCaughtBall()
    {
        return ballOwnerShipHandlerGameObject;
    }
}