using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateRemainingTimeText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text textToUpdate;
    
    [Header("Config")] 
    private const String remainingTimeTextFormating = "F1";


    public void UpdateRemainingTime(float remainingTime)
    {
        textToUpdate.text = remainingTime.ToString(remainingTimeTextFormating);
    }
}