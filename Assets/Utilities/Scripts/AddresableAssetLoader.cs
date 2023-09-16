using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableAssetLoader
{
    private AssetReference assetReference;
    private GameObject assetReferenceInstance;

    private Action<GameObject> onLoaded;

    public AddressableAssetLoader(AssetReference assetReference)
    {
        this.assetReference = assetReference;
    }

    public void LoadAsync(Action<GameObject> onLoaded)
    {
        this.onLoaded = onLoaded;
        var asyncOperationHandler = assetReference.InstantiateAsync();
        asyncOperationHandler.Completed += HandleLoadedReference;
    }

    private void HandleLoadedReference(AsyncOperationHandle<GameObject> obj)
    {
        assetReferenceInstance = obj.Result;
        onLoaded?.Invoke(assetReferenceInstance);
    }

    public void ReleaseInstance()
    {
        Addressables.ReleaseInstance(assetReferenceInstance);
    }

    public void ReleaseCompletely()
    {
        Addressables.ReleaseInstance(assetReferenceInstance);
        assetReference.ReleaseAsset();
    }
}