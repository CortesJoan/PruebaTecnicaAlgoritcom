using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterOnCollisionEnter : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Transform placeToTeleport;
    [SerializeField] private string tagToTeleport;


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(tagToTeleport))
        {
            other.transform.position = placeToTeleport.position;
            var ballRigidbody = other.transform.GetComponentInParent<Rigidbody>();
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
        }
    }
}