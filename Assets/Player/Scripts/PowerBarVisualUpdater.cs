using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarVisualUpdater : MonoBehaviour
{
    [SerializeField] Image powerBarFiller;
    [SerializeField] private float lowPowerThreshold = 0.25f;
    [SerializeField] private float midPowerThreshold = 0.5f;
    [SerializeField] private float highPowerThreshold = 0.75f;
    [SerializeField] private Color lowPowerColor = Color.red;
    [SerializeField] private Color midPowerColor = Color.yellow;
    [SerializeField] private Color highPowerColor = Color.green;
    
    public void OnPowerChanged(float newPower)
    {
        powerBarFiller.fillAmount = newPower;
        AdjustColorWithNewPower(newPower);
    }

    private void AdjustColorWithNewPower(float newPower)
    {
        if (newPower < lowPowerThreshold)
            powerBarFiller.color = lowPowerColor;
        else if (newPower <= midPowerThreshold)
            powerBarFiller.color = midPowerColor;
        else
            powerBarFiller.color = highPowerColor;
    }
}