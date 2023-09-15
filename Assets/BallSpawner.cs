using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private AssetReference ballReference;
    private List<GameObject> ballInstances = new List<GameObject>();
    [SerializeField] private List<AddressableAssetLoader> addressableAssetLoaders = new List<AddressableAssetLoader>();

    void Start()
    {
        SpawnBall();
    }

    [ContextMenu("SpawnBall")]
    void SpawnBall()
    {
        AddressableAssetLoader addressableAssetLoader = new AddressableAssetLoader(ballReference);
        addressableAssetLoader.LoadAsync(OnBallLoaded);
        addressableAssetLoaders.Add(addressableAssetLoader);
    }

    public void OnBallLoaded(GameObject ball)
    {
        ballInstances.Add(ball);
    }

    private void OnDestroy()
    {
        foreach (var addressableAssetLoader in addressableAssetLoaders)
        {
            addressableAssetLoader.ReleaseCompletely();
        }
    }
}