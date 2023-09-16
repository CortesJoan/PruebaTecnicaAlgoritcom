using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerPossessionCounter : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float maxPossessionTime;
    
    [SerializeField] private UnityEvent onReachedMaxPossessionTime;
    [SerializeField] private UnityEvent<float> onPossessionTimeUpdated;
    private TimerWithDelegate possessionTimer;
    private bool isTimerRunning = false;

    public void OnPossessionStarted()
    {
        StartPossessionTimer();
    }

    private void StartPossessionTimer()
    {
        possessionTimer = new TimerWithDelegate(maxPossessionTime,reverseTimer:true);
        possessionTimer.SetDelegateFunction(FinishPossession);
        isTimerRunning = true;
    }

    private void Update()
    {
        if (!isTimerRunning) return;
        possessionTimer.Update(-Time.deltaTime);
        onPossessionTimeUpdated.Invoke(possessionTimer.GetCurrentTime());
    }

    private void FinishPossession()
    {
        onReachedMaxPossessionTime?.Invoke();
        isTimerRunning = false;
    }

    public void InterruptPossessionTimer()
    {
        possessionTimer.Reset();  
        isTimerRunning = false;
    }
}