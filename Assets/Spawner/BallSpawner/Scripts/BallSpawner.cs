using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BallSpawner : MonoBehaviour, ISpawner
{
    [Header("Config")] [SerializeField] private AssetReference ballReference;
    [SerializeField] private int ballsToSpawnOnStart = 1;
    private List<GameObject> ballInstances = new List<GameObject>();
    [SerializeField] private List<AddressableAssetLoader> addressableAssetLoaders = new List<AddressableAssetLoader>();

    void Start()
    {
        for (int i = 0; i < ballsToSpawnOnStart; i++)
        {
            Spawn();
        }
    }

    [ContextMenu("SpawnBall")]
    public void Spawn()
    {
        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(ballReference);
        addressableAssetLoader.LoadAsync(OnBallLoaded);
        addressableAssetLoaders.Add(addressableAssetLoader);
    }

    public Vector3 GetSpawnPosition()
    {
        return transform.position;
    }

    public void OnBallLoaded(GameObject ball)
    {
        ballInstances.Add(ball);
        ball.GetComponent<SpawnedBy>().SetSpawner(this);
    }

    private void OnDestroy()
    {
        foreach (var addressableAssetLoader in addressableAssetLoaders)
        {
            addressableAssetLoader.ReleaseCompletely();
        }
    }
}
