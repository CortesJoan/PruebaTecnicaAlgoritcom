using UnityEngine;

public class TimerWithDelegate {
    public delegate void TimeDelegate();
    TimeDelegate automaticCallBack;
    float maxTime;
    public bool autoResets;
    public bool reverseTimer;
    public float currentTime { get; private set; }
    public bool timesUp {
        get { return TimesUp(); }
    }
    bool automaticFlag;

    private bool TimesUp() {
        return reverseTimer? currentTime <= 0: currentTime >= maxTime;
    }
 
    public void Update() {
        if (timesUp) return;

        currentTime += Time.deltaTime;
        if (TimesUp()) {
            if (automaticCallBack != null) {
                if (!automaticFlag) {
                    automaticFlag = true;
                    automaticCallBack();
                }
                if (autoResets) {
                    automaticFlag = false;
                    currentTime = 0;
                }
            }
        }
    }

 
    public void Update(float deltaTime) {
        if (timesUp) return;

        currentTime += deltaTime;
        if (TimesUp()) {
            if (automaticCallBack != null) {
                if (!automaticFlag) {
                    automaticFlag = true;
                    automaticCallBack();
                }
                if (autoResets) {
                    automaticFlag = false;
                    currentTime = reverseTimer? maxTime :0;
                }
            }
        }
    }

    public void SetDelegateFunction(TimeDelegate a) {
        automaticCallBack = a;
    }

    public void SetMaxTime(float max, bool mustReset = false) {
        maxTime = max;
        if (mustReset) Reset();
    }

    public float GetMaxTime() {
        return maxTime;
    }

    public void Reset(float newTime = -1f) {
        if (newTime >= 0) {
            SetMaxTime(newTime);
        }
        currentTime = reverseTimer ? maxTime : 0;
        automaticFlag = false;
    }

    public void SetCurrentTime(float newT) {
        currentTime = newT;
    }
    public float GetCurrentTime() {
        return currentTime;
    }

    public void SetTimerToEnd(bool triggerCallback) {
        SetCurrentTime(reverseTimer ? 0 : maxTime);
        automaticFlag = triggerCallback;
    }

 
    public float GetProgress() {
        return currentTime / maxTime;
    }

    public TimerWithDelegate(float maxTime) {
        this.maxTime = maxTime;
        autoResets = false;
    }

    
    public TimerWithDelegate(float maxTime, bool autoResets=false,bool reverseTimer=false) {
        this.maxTime = maxTime;
        this.autoResets = autoResets;
        this.reverseTimer = reverseTimer;
        if (reverseTimer)
        {
            currentTime = this.maxTime;
        }
    }
}