using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private float currentPoints;
    [SerializeField] private UnityEvent<float> onPointsChanged;

    public void AddPoints(int points)
    {
        currentPoints += points;
        onPointsChanged?.Invoke(currentPoints);
    }
}