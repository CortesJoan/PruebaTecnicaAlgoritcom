using UnityEngine;
using UnityEngine.AddressableAssets;

public class TrajectorySimulator
{
    private AssetReference fakeBallReference;
    private GameObject fakeBallInstance;
    private Rigidbody fakeBallRigidbody;
    private LineRenderer fakeBallTrialRenderer;
    private AddressableAssetLoader addressableAssetLoader;
    private Vector3 direction;
    private Transform simulationRelativeTo;
    
    private const int linePoints = 25;
    private const float timeBetweenPoints = 0.1f;

    public TrajectorySimulator(AssetReference fakeBallReference, Transform simulationRelativeTo)
    {
        addressableAssetLoader = new AddressableAssetLoader(fakeBallReference);
        addressableAssetLoader.LoadAsync(HandleLoadedFakeBall);
        this.simulationRelativeTo = simulationRelativeTo;
    }

    private void HandleLoadedFakeBall(GameObject newFakeBall)
    {
        fakeBallInstance = newFakeBall;
        fakeBallTrialRenderer = fakeBallInstance.GetComponent<LineRenderer>();
        fakeBallRigidbody = fakeBallInstance.GetComponent<Rigidbody>();
    }

    public void ThrowRigidbodyInTrajectory(Rigidbody ballRigidbody, float powerRatio,
        float maxThrowingForce, ForceMode forceMode = ForceMode.Impulse)
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        var force = powerRatio * maxThrowingForce;
        ballRigidbody.AddForce(direction * force, forceMode);
    }


    public void DrawTrajectory(Vector3 ballPosition,   float powerRatio,
        float maxThrowingForce = 1000f, LayerMask layerMask = new LayerMask())
    {
        if (!fakeBallTrialRenderer)
        {
            return;
        }
        if (direction == Vector3.zero)
        {
            direction = simulationRelativeTo.transform.TransformDirection(Vector3.forward + Vector3.up);
        }
        fakeBallTrialRenderer.positionCount = 0;
        fakeBallTrialRenderer.enabled = false;
        fakeBallTrialRenderer.enabled = true;
        fakeBallTrialRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = ballPosition;
        Vector3 startVelocity = maxThrowingForce * (direction * powerRatio) / fakeBallRigidbody.mass;
        int i = 0;
        fakeBallTrialRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            fakeBallTrialRenderer.SetPosition(i, point);

            Vector3 lastPosition = fakeBallTrialRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                    (point - lastPosition).normalized,
                    out RaycastHit hit,
                    (point - lastPosition).magnitude,
                    layerMask))
            {
                fakeBallTrialRenderer.SetPosition(i, hit.point);
                fakeBallTrialRenderer.positionCount = i + 1;
                return;
            }
        }
    }

    public void ChangeXTrajectory(float changeByValue)
    {
        if (simulationRelativeTo)
        {
            Vector3 localDirection = simulationRelativeTo.transform.InverseTransformDirection(direction);
            localDirection.x += changeByValue;
            direction = simulationRelativeTo.transform.TransformDirection(localDirection);
        }
        else
        {
            direction.x += changeByValue;
        }
    }

    public void Cleanup()
    {
        addressableAssetLoader.ReleaseCompletely();
    }
}