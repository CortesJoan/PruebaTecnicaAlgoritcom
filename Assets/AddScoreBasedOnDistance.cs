using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScoreBasedOnDistance : MonoBehaviour
{
    [SerializeField] private int pointsToAddSmallDistance = 1;
    [SerializeField] private float minimumDistanceForMidDistancePoints = 4f;
    [SerializeField] private int pointsToAddMidDistance = 2;
    [SerializeField] private float minimumDistanceForFarDistancePoints = 8f;
    [SerializeField] private int pointsToAddFarDistance = 3;

    public void OnBallEntered(GameObject ball)
    {
        var ballOwnershipHandler = ball.GetComponentInChildren<BallOwnershipHandler>();
        var previousOwner = ballOwnershipHandler.GetPreviousOwner();
        float currentDistance = Vector3.Distance(previousOwner.GetLastThrowPosition(), ball.transform.position);
        Debug.Log(currentDistance);
        var playerScore = previousOwner.GetComponent<PlayerScore>();
        if (currentDistance<minimumDistanceForMidDistancePoints)
        {
            playerScore.AddPoints(pointsToAddSmallDistance);
        }else if (currentDistance < minimumDistanceForFarDistancePoints)
        {
            playerScore.AddPoints(pointsToAddMidDistance);
            
        }
        else
        {
            playerScore.AddPoints(pointsToAddFarDistance);
        }
    }
}