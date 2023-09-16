using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketballHoopController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject hoop;
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private string ballTag= "";
    
    [Header("Events")]
    public UnityEvent<GameObject> onBallEntered;
    private void OnTriggerEnter(Collider other)
    {
        if (  !other.CompareTag(ballTag) || !other.isTrigger)
        {
            return;
        } 
        Vector3 direction = other.GetComponentInParent<Rigidbody>().velocity;
    
        if (direction.y <0     && other.isTrigger)
        {
            onBallEntered?.Invoke(other.gameObject);
            other.GetComponentInParent<SpawnedBy>().ReturnToSpawnPosition();
        }
    }

    public GameObject GetTargetPoint()
    {
        return targetPoint;
    }
}
