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
    [SerializeField] private Color highPowerColor = Color.green;
    [SerializeField] private Color midPowerColor = Color.yellow;


    public void OnPowerChanged(float newPower)
    {
        powerBarFiller.fillAmount = newPower;
        AdjustColorWithNewPower(newPower);
    }

    void AdjustColorWithNewPower(float newPower)
    {
        if (newPower < lowPowerThreshold)
            powerBarFiller.color = Color.red;
        else if (newPower <= midPowerThreshold)
            powerBarFiller.color = Color.yellow;
        else
            powerBarFiller.color = Color.green;
    }
}