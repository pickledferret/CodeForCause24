using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform m_nextTrackRaycastPoint;
    [SerializeField] private LayerMask m_trackMask;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_rotationSpeed = 5f;

    [SerializeField] private float m_positionThreshold = 0.01f;
    [SerializeField] private float m_rotationThreshold = 5f;

    private TrackBase m_currentTrack;
    private TrackBase m_nextTrack;
    private bool m_isMoving = false;
    private List<Transform> m_waypoints = new List<Transform>();
    private int m_currentWaypointIndex = 0;

    void Start()
    {
    }

    void Update()
    {
        if (m_isMoving)
        {
            UpdatePosition();
            UpdateRotation();
        }
        RaycastCheckForTrackChange();
    }

    private void RaycastCheckForTrackChange()
    {
        Ray ray = new Ray(m_nextTrackRaycastPoint.position, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.25f, m_trackMask))
        {
            TrackBase track = hit.transform.GetComponent<TrackBase>();
            if (track && track != m_currentTrack)
            {
                m_nextTrack = track;

                if (m_currentTrack == null)
                {
                    TrackChanged(track);
                }
            }
        }
    }

    private void TrackChanged(TrackBase track)
    {
        m_isMoving = true;
        m_currentTrack = track;
        SetWaypoints(track.GetWaypoints());
    }

    private void SetWaypoints(List<Transform> waypoints)
    {
        if (waypoints == null || waypoints.Count == 0) return;

        m_waypoints = waypoints;
        m_currentWaypointIndex = 1;
        m_isMoving = true;
    }

    private void UpdatePosition()
    {
        if (m_currentTrack == null || m_waypoints.Count == 0) return;

        Vector3 target = m_waypoints[m_currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, target, m_speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < m_positionThreshold)
        {
            m_currentWaypointIndex++;
            if (m_currentWaypointIndex >= m_waypoints.Count)
            {
                if (m_nextTrack != null)
                {
                    TrackChanged(m_nextTrack);
                    m_nextTrack = null;
                }
                else
                {
                    m_isMoving = false;
                }
            }
        }
    }

    private void UpdateRotation()
    {
        if (m_currentWaypointIndex >= m_waypoints.Count) return;

        Transform targetWaypoint = m_waypoints[m_currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        Vector3 upDirection = targetWaypoint.up;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, upDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
        }
    }
}