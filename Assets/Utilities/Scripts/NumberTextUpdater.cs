using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateTextNumber : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TMP_Text textToUpdate;

    [Header("Config")] [SerializeField] private string textBeforeTheNumber = "";
    [SerializeField] private string textAfterTheNumber = "";
    [SerializeField] private string textFormatting = "F1";


    public void UpdateNumber(float numberToPutInText)
    {
        textToUpdate.SetText(textBeforeTheNumber + numberToPutInText.ToString(textFormatting) + textAfterTheNumber);  
    }
    public void UpdateWholeText(string newText)
    {
        textToUpdate.text = newText;
    }
}