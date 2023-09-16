using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class PlayerBallHandler : MonoBehaviour
{
    [Header("References")] private Rigidbody ballRigidbody;

    [Header("Config")] [SerializeField] private Transform ballHolder;
    [SerializeField] private AssetReference ballVisualsReference;
    [Header("Debug")] private bool playerHasBall;
    private BallOwnershipHandler ballOwnershipHandler;
    GameObject ballVisualsInstance;
    private Vector3 lastThrowPosition;
    [SerializeField] private UnityEvent onLostBall;

    public void GiveBall(GameObject newBall)
    {
        ballOwnershipHandler = newBall.GetComponentInParent<BallOwnershipHandler>(true);
        ballOwnershipHandler.SetOwner(this);
        playerHasBall = true;
        newBall.transform.rotation = Quaternion.identity;
//        this.newBall.transform.DOMove(ballHolder.transform.position, .15f).OnComplete(() => newBall.transform.parent = ballHolder).on.OnUpdate(()=> Debug.Log(""));
        this.ballOwnershipHandler.transform.position = ballHolder.transform.position;
        newBall.transform.parent = ballHolder;
        newBall.transform.localPosition = Vector3.zero;
        newBall.SetActive(false);
        var asyncOperationHandler = ballVisualsReference.InstantiateAsync();
        asyncOperationHandler.Completed += HandleLoadedBallVisuals;
        //  ballHolder.transform.DOMove(,1f).From();
    }

    private void HandleLoadedBallVisuals(AsyncOperationHandle<GameObject> obj)
    {
        ballVisualsInstance = obj.Result;
        ballVisualsInstance.transform.parent = ballHolder;
        ballVisualsInstance.transform.localPosition = Vector3.zero;
    }

    public void LoseBall(bool returnBallToSpawnPosition)
    {
        ReleaseBall();
        if (returnBallToSpawnPosition)
        {
            ballOwnershipHandler.GetComponent<SpawnedBy>().ReturnToSpawnPosition();
        } 
        ballRigidbody = null;
        playerHasBall = false;
        onLostBall?.Invoke();
    }

    void ReleaseBall()
    {
        if (ballVisualsInstance)
        {
            ballOwnershipHandler.transform.position = ballVisualsInstance.transform.position;
            Addressables.ReleaseInstance(ballVisualsInstance);
            ballVisualsReference.ReleaseAsset();
            ballVisualsInstance = null;
        }
        if (ballOwnershipHandler)
        {
            ballOwnershipHandler.transform.parent = null;
            ballOwnershipHandler.gameObject.SetActive(true);
        }
    }

    public bool HasBall()
    {
        return playerHasBall;
    }

    public GameObject GetBall()
    {
        return ballOwnershipHandler.gameObject;
    }

    public GameObject GetBallVisuals()
    {
        return ballVisualsInstance;
    }

    public void SetLastThrowPosition(Vector3 position)
    {
        lastThrowPosition = position;
    }

    public Vector3 GetLastThrowPosition()
    {
        return lastThrowPosition;
    }
}