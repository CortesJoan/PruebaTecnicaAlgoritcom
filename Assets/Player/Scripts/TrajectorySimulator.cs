using UnityEngine;
using UnityEngine.AddressableAssets;

public class TrajectorySimulator
{
    private AssetReference fakeBallReference;
    private GameObject trajectoryVisualInstance;
    private Rigidbody ballRigidbody;
    private LineRenderer trajectoryVisualLineRenderer;
    private AddressableAssetLoader addressableAssetLoader;
    private Vector3 direction;
    private Transform simulationRelativeTo;
    
    private const int linePoints = 25;
    private const float timeBetweenPoints = 0.1f;

    public TrajectorySimulator(AssetReference trajectoryVisualAssetReference, Transform simulationRelativeTo,Rigidbody ballRigidbody)
    {
        addressableAssetLoader = new AddressableAssetLoader(trajectoryVisualAssetReference);
        addressableAssetLoader.LoadAsync(HandleLoadedFakeBall);
        this.simulationRelativeTo = simulationRelativeTo;
        this.ballRigidbody = ballRigidbody; 
    }

    private void HandleLoadedFakeBall(GameObject newFakeBall)
    {
        trajectoryVisualInstance = newFakeBall;
        trajectoryVisualLineRenderer = trajectoryVisualInstance.GetComponent<LineRenderer>();
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
        if (!trajectoryVisualLineRenderer)
        {
            return;
        }
        if (direction == Vector3.zero)
        {
            direction = simulationRelativeTo.transform.TransformDirection(Vector3.forward + Vector3.up);
        }
        trajectoryVisualLineRenderer.positionCount = 0;
        trajectoryVisualLineRenderer.enabled = false;
        trajectoryVisualLineRenderer.enabled = true;
        trajectoryVisualLineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startPosition = ballPosition;
        Vector3 startVelocity = maxThrowingForce * (direction * powerRatio) / ballRigidbody.mass;
        int i = 0;
        trajectoryVisualLineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            trajectoryVisualLineRenderer.SetPosition(i, point);

            Vector3 lastPosition = trajectoryVisualLineRenderer.GetPosition(i - 1);

            if (Physics.Raycast(lastPosition,
                    (point - lastPosition).normalized,
                    out RaycastHit hit,
                    (point - lastPosition).magnitude,
                    layerMask))
            {
                trajectoryVisualLineRenderer.SetPosition(i, hit.point);
                trajectoryVisualLineRenderer.positionCount = i + 1;
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