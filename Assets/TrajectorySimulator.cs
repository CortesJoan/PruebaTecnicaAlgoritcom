using UnityEngine;
using UnityEngine.AddressableAssets;

public class TrajectorySimulator
{
    private AssetReference fakeBallReference;
    private GameObject fakeBallInstance;
    private Rigidbody fakeBallRigidbody;
    private TrailRenderer fakeBallTrialrenderer;
    private AddressableAssetLoader addressableAssetLoader;

    public TrajectorySimulator(AssetReference fakeBallReference)
    {
        addressableAssetLoader = new AddressableAssetLoader(fakeBallReference);

        addressableAssetLoader.LoadAsync(HandleLoadedFakeBall);
    }

    private void HandleLoadedFakeBall(GameObject newFakeBall)
    {
        fakeBallInstance = newFakeBall;
        fakeBallRigidbody = fakeBallInstance.GetComponent<Rigidbody>();
        fakeBallTrialrenderer = fakeBallInstance.GetComponent<TrailRenderer>();
    }

    public void SimulateTrajectory(Vector3 ballPosition, Vector3 targetPosition, float powerRatio,
        float minimumYDirection = 1, float maxThrowingForce = 1000f)
    {
        if (!fakeBallInstance)
        {
            return;
        }
        fakeBallInstance.transform.position = ballPosition;
        fakeBallTrialrenderer.Clear();

        Vector3 direction = (targetPosition - ballPosition).normalized;
        direction.y = minimumYDirection;
        ThrowRigidbodyInTrajectory(fakeBallRigidbody, direction, powerRatio, maxThrowingForce);
    }

    public void ThrowRigidbodyInTrajectory(Rigidbody ballRigidbody, Vector3 direction, float powerRatio,
        float maxThrowingForce, ForceMode forceMode = ForceMode.Impulse)
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        var force = powerRatio * maxThrowingForce;
        ballRigidbody.AddForce(direction * force, forceMode);
    }

    public void Cleanup()
    {
        addressableAssetLoader.ReleaseCompletely();
    }
}