using UnityEngine;
using UnityEngine.Events;

public class PowerBarController : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private UnityEvent<float> onPowerChanged;
    [SerializeField] private float currentPower;

    public float GetCurrentPower()
    {
        return currentPower;
    }

    public void SetCurrentPower(float newPower)
    {
        currentPower = newPower;
        onPowerChanged?.Invoke(currentPower);
    }
}