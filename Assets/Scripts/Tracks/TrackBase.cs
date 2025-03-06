using System.Collections.Generic;
using UnityEngine;

public abstract class TrackBase : MonoBehaviour
{
    [SerializeField] protected List<Transform> m_waypointNodes = new();

    public Transform GetStartPoint()
    {
        return m_waypointNodes[0];
    }

    public List<Transform> GetWaypoints()
    {
        return m_waypointNodes;
    }

    private void OnDrawGizmos()
    {
        if (m_waypointNodes.Count > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < m_waypointNodes.Count; i++)
            {
                if (i == m_waypointNodes.Count - 1)
                    return;

                if (m_waypointNodes[i] == null || m_waypointNodes[i + 1] == null)
                    return;

                Gizmos.DrawLine(m_waypointNodes[i].position, m_waypointNodes[i + 1].position);
            }
        }
    }
}