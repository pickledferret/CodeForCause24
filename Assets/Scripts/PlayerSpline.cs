using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerSpline : MonoBehaviour
{
    public SplineContainer m_spline;
    public float speed = 1f;

    float distancePercentage = 0f;
    float splineLength;

    void Start()
    {
        splineLength = m_spline.CalculateLength();

        Vector3 localSplinePoint = m_spline.transform.InverseTransformPoint(transform.position);

        // Get the closest point and corresponding t value on the spline
        SplineUtility.GetNearestPoint(m_spline.Spline, localSplinePoint, out float3 nearestPoint, out float nearestT);
        // Convert back to world space
        Vector3 nearestWorldPosition = m_spline.transform.TransformPoint(nearestPoint);
        // Set the starting position and distance percentage based on nearestT
        transform.position = nearestWorldPosition;
        distancePercentage = nearestT;
    }

    void Update()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;
        distancePercentage = Mathf.Clamp01(distancePercentage);

        Vector3 currentPos = m_spline.EvaluatePosition(distancePercentage);
        transform.position = currentPos;

        Vector3 nextPos = m_spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 dir = nextPos - currentPos;
        transform.rotation = Quaternion.LookRotation(dir, transform.up);
    }
}
