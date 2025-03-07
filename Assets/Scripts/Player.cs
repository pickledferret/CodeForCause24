using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Transform m_nextTrackRaycastPoint;
    [SerializeField] private LayerMask m_trackMask;
    [SerializeField] private float m_speed = 5f;

    private bool m_isMoving = true;
    private bool m_crashed = false;

    private SplineContainer m_currentSpline;
    private float m_splineDistancePercentage = 0f;
    private float m_splineLength = 0f;
    private bool m_splineCompleted = false;

    // Speed Boost Controls
    private float m_originalSpeed;
    private float m_currentSpeed;
    private Coroutine m_speedBoostCoroutine;
    private Coroutine m_speedDebuffCoroutine;

    void Start()
    {
        m_originalSpeed = m_speed;
        m_currentSpeed = m_originalSpeed;

    }

    void Update()
    {
        if (m_crashed)
            return;

        CheckForUserInput();

        if (m_isMoving && m_currentSpline)
        {
            UpdatePosition();
        }

        if (m_splineCompleted || (m_currentSpline == null && m_isMoving))
        {
            RaycastCheckForTrackChange();
        }
    }

    public void Crashed(float heightForceMultiplier = 1f)
    {
        ClearCurrentSpline();
        m_crashed = true;
        m_isMoving = false;
        m_rigidbody.isKinematic = false;

        m_rigidbody.AddForce(transform.forward + (transform.up * heightForceMultiplier), ForceMode.Impulse);

        m_rigidbody.angularVelocity = Vector3.zero;
        Vector3 flipTorque = transform.right * 10f;
        m_rigidbody.AddTorque(flipTorque, ForceMode.Force);
    }

    private void CheckForUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameplayEvents.OnUserInputPressed();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            GameplayEvents.OnUserInputReleased();
        }
    }

    private void RaycastCheckForTrackChange()
    {
        Ray ray = new Ray(m_nextTrackRaycastPoint.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f, m_trackMask))
        {
            TrackSpline trackSpline = hit.transform.GetComponent<TrackSpline>();
            if (trackSpline && trackSpline.GetSpline() != m_currentSpline)
            {
                m_currentSpline = trackSpline.GetSpline();
                TrackChanged(m_currentSpline);
                m_splineCompleted = false;
            }
        }
        else
        {
            Crashed();
        }
    }

    private void TrackChanged(SplineContainer newSpline)
    {
        m_isMoving = true;
        m_currentSpline = newSpline;

        m_splineLength = m_currentSpline.CalculateLength();

        Vector3 localSplinePoint = m_currentSpline.transform.InverseTransformPoint(transform.position);
        SplineUtility.GetNearestPoint(m_currentSpline.Spline, localSplinePoint, out float3 nearestPoint, out float nearestT);
        Vector3 nearestWorldPosition = m_currentSpline.transform.TransformPoint(nearestPoint);
        m_splineDistancePercentage = nearestT;

        if (m_currentSpline == null)
        {
            transform.position = nearestWorldPosition;
        }
    }

    private void UpdatePosition()
    {
        m_splineDistancePercentage += m_currentSpeed * Time.deltaTime / m_splineLength;
        m_splineDistancePercentage = Mathf.Clamp01(m_splineDistancePercentage);

        if (m_splineDistancePercentage >= 1f)
        {
            m_splineCompleted = true;
            RaycastCheckForTrackChange();
        }

        if (m_currentSpline != null)
        {
            Vector3 currentPos = m_currentSpline.EvaluatePosition(m_splineDistancePercentage);
            transform.position = currentPos;

            Vector3 nextPos = m_currentSpline.EvaluatePosition(m_splineDistancePercentage + 0.05f);
            Vector3 dir = nextPos - currentPos;
            transform.rotation = Quaternion.LookRotation(dir, transform.up);
        }
    }

    public float GetCurrentSplineDistancePercentage()
    {
        return m_splineDistancePercentage;
    }

    private void ClearCurrentSpline()
    {
        m_currentSpline = null;
        m_splineCompleted = false;
    }

    private IEnumerator SpeedDebuffCoroutine(float newSpeed)
    {
        float elapsedTime = 0f;
        float startSpeed = m_currentSpeed;

        while (startSpeed > newSpeed)
        {
            m_currentSpeed = Mathf.Lerp(startSpeed, newSpeed, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_speedBoostCoroutine = null;
    }

    public void ApplySpeedBoostMultiplier(float newSpeed, float duration)
    {
        if (m_speedBoostCoroutine != null)
        {
            StopCoroutine(m_speedBoostCoroutine);
        }

        m_speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(newSpeed, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float newSpeed, float duration)
    {
        float elapsedTime = 0f;
        float startSpeed = m_currentSpeed;

        // phase durations
        float accelerationDuration = duration / 10f; // 1/10
        float holdDuration = duration * 3f / 5f; // 3/5
        float decelerationDuration = duration * 3f / 10f; // 3/10


        // Acceleration
        while (elapsedTime < accelerationDuration)
        {
            m_currentSpeed = Mathf.Lerp(startSpeed, newSpeed, elapsedTime / accelerationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_currentSpeed = newSpeed;

        // Hold Speed
        yield return new WaitForSeconds(holdDuration);

        // Deceleration
        elapsedTime = 0f;
        startSpeed = m_currentSpeed;

        while (elapsedTime < decelerationDuration)
        {
            m_currentSpeed = Mathf.Lerp(startSpeed, m_originalSpeed, elapsedTime / decelerationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_currentSpeed = m_originalSpeed;

        m_speedBoostCoroutine = null;
    }

    public void JumpTo(Vector3 end, float jumpHeight, float jumpDuration, bool crashed)
    {
        ClearCurrentSpline();
        StartCoroutine(JumpToPoint(transform.position, end, jumpHeight, jumpDuration, crashed));
    }

    private IEnumerator JumpToPoint(Vector3 start, Vector3 end, float height, float duration, bool crashed)
    {
        float elapsedTime = 0f;

        m_isMoving = false;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            Vector3 position = Vector3.Lerp(start, end, t);
            position.y += height * (1 - Mathf.Pow(2 * t - 1, 2));
            transform.position = position;

            float rotation = Mathf.Lerp(0, 360, t);
            transform.rotation = Quaternion.Euler(rotation, 0, 0);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (crashed)
        {
            Crashed();
        }
        else 
        {
            m_isMoving = true;
        }

        transform.position = end;
        transform.rotation = Quaternion.Euler(360, 0, 0);
    }
}