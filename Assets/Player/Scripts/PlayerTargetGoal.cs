using UnityEngine;

public class PlayerTargetGoal : MonoBehaviour
{
    [Header("Config")] 
    [SerializeField] private GameObject targetGoal;

    public void SetTargetGoal(GameObject targetGoal)
    {
        this.targetGoal = targetGoal;
        
    }

    public GameObject GetTargetGoal()
    {
        return targetGoal;
    }
}