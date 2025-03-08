using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_followTarget;
    [Space(10)]
    [SerializeField] private Vector3 m_followOffset = new Vector3(0, 5, -10);
    [SerializeField] private float m_followSmoothSpeed = 5f;
    [Space(10)]
    [SerializeField] private Vector3 m_lookAtOffset = Vector3.zero;
    [SerializeField] private float m_lookAtSmoothSpeed = 5f;

    private bool m_canUpdate = false;
    private bool m_ignoreTargetRotation = false;

    public void SetFollowTarget(Transform target)
    { 
        m_followTarget = target;
    }

    public void CanUpdate(bool canUpdate)
    {
        m_canUpdate = canUpdate;
    }

    public void IgnoreTargetRotation(bool follow)
    {
        m_ignoreTargetRotation = follow;
    }

    void LateUpdate()
    {
        if (m_followTarget == null || !m_canUpdate)
            return;

        Quaternion yRotation = Quaternion.Euler(0, m_followTarget.eulerAngles.y, 0);
        Vector3 rotatedFollowOffset = m_ignoreTargetRotation ? m_followOffset : yRotation * m_followOffset;
        Vector3 desiredPosition = m_followTarget.position + rotatedFollowOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * m_followSmoothSpeed);

        Vector3 lookAtPosition = m_followTarget.position + (m_ignoreTargetRotation ? m_lookAtOffset : m_followTarget.rotation * m_lookAtOffset);
        Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_lookAtSmoothSpeed);
    }

    void OnValidate()
    {
        if (m_followTarget != null)
        {
            transform.position = m_followTarget.position + m_followOffset;
            transform.LookAt(m_followTarget.position + m_lookAtOffset);
        }
    }
}