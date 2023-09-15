using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CatchBall : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private string ballTag = "Ball";
    
    [Header("Events")]
    public UnityEvent<GameObject> onBallCatched;
    public UnityEvent<GameObject> onBallLosed;

    private GameObject ballOwnerShipHandlerGameObject;
    private bool canCatchBall;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ballTag) && other)
        {
            var ballOwnershipHandler = other.GetComponentInParent<BallOwnershipHandler>();
            if (ballOwnershipHandler.HasOwner())
            {
                return;
            }
            CatchBallWithOwnership(ballOwnershipHandler);
        }
    }

    public void CatchBallIfDetected(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
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
        onBallCatched?.Invoke(ballOwnerShipHandlerGameObject);
    }
    public void LoseBall()
    {
        onBallLosed?.Invoke(ballOwnerShipHandlerGameObject);
        ballOwnerShipHandlerGameObject = null;
    }

    public GameObject GetCaughtBall()
    {
        return ballOwnerShipHandlerGameObject;
    }
}