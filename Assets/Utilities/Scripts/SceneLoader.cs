using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private AssetReference sceneToLoad;
    public void ReloadCurrentScene()
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneToLoad);

        handle.Completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        var scene = obj.Result;
        scene.ActivateAsync();
    }
}