using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PlayerTargetBasket : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private GameObject targetBasket; 
    public UnityEvent onTargetBasketChanged;
    public void SetTargetBasket(GameObject targetBasket)
    {
        this.targetBasket = targetBasket;
        onTargetBasketChanged.Invoke();
    }

    public GameObject GetTargetBasket()
    {
        return targetBasket;
    }
}