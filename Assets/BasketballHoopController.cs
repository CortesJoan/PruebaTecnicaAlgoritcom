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
    [SerializeField] private string ballTag= "Ball";
    
    [Header("Events")]
    public UnityEvent<GameObject> onBallEntered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.y<hoop.transform.position.y)
        {
            return;
        }
        if (other.CompareTag(ballTag) && !other.isTrigger)
        {
            onBallEntered?.Invoke(other.gameObject);
        }
    }

    public GameObject GetTargetPoint()
    {
        return targetPoint;
    }
}
