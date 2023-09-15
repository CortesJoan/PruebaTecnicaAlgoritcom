using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTeleporterTrigger : MonoBehaviour
{
    [SerializeField] private Transform placeToTeleport;

 

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            other.transform.position = placeToTeleport.position;
            var ballRigidbody = other.transform.GetComponentInParent<Rigidbody>();
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;

        }
    }
}