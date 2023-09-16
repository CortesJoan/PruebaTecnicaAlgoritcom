using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerName : MonoBehaviour
{

    [SerializeField] private string playerName;

    [SerializeField] private UnityEvent<string> onNameChanged;
    public void SetPlayerName(string newName)
    {
        playerName = newName;
        onNameChanged?.Invoke(newName);
    }
}
