using UnityEngine;

public class PlayerTargetGoal : MonoBehaviour
{
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