using UnityEngine;
using UnityEngine.Events;

public class AddScoreBasedOnDistance : MonoBehaviour
{
    [SerializeField] private int pointsToAddSmallDistance = 1;
    [SerializeField] private float minimumDistanceForMidDistancePoints = 4f;
    [SerializeField] private int pointsToAddMidDistance = 2;
    [SerializeField] private float minimumDistanceForFarDistancePoints = 8f;
    [SerializeField] private int pointsToAddFarDistance = 3;
    [SerializeField] public UnityEvent onScoreAdded;

    public void OnBallEntered(GameObject ball)
    {
        var ballOwnershipHandler = ball.GetComponentInParent<BallOwnershipHandler>();
        var previousOwner = ballOwnershipHandler.GetPreviousOwner();
        if (!previousOwner)
        {
            return;
        }
        float currentDistance = Vector3.Distance(previousOwner.GetLastThrowPosition(), ball.transform.position);
        Debug.Log(currentDistance);
        var playerScore = previousOwner.GetComponent<PlayerScore>();
        playerScore.AddPoints(CalcScoreBasedOnDistance(currentDistance));
        onScoreAdded?.Invoke();
    }

    int CalcScoreBasedOnDistance(float currentDistance)
    {
        if (currentDistance < minimumDistanceForMidDistancePoints)
        {
            return pointsToAddSmallDistance;
        }
        if (currentDistance < minimumDistanceForFarDistancePoints)
        {
            return
                pointsToAddMidDistance;
        }
        return pointsToAddFarDistance;
    }
}