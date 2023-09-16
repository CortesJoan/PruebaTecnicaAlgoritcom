using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedBy : MonoBehaviour
{
    ISpawner spawner;

    public void SetSpawner(ISpawner spawner)
    {
        this.spawner = spawner;
    }
    public void ReturnToSpawnPosition()
    {
        this.transform.position = spawner.GetSpawnPosition();
    }
}
