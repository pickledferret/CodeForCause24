using DG.Tweening;
using UnityEngine;

public class TrackRotator : TrackBase
{
    [SerializeField] private DirectionState m_directionState;
    [SerializeField] private StartState m_startState;
    [SerializeField] private Transform m_rotatorPivot;
    [SerializeField] private float m_rotateTweenDuration = 0.75f;

    [Header("Player Override Settings")]
    [SerializeField] private float m_speedOverride = 0.5f;
    [SerializeField] private float m_speedOverrideDuration = 5f;

    [System.Serializable]
    private enum DirectionState { ROTATE_LEFT, ROTATE_RIGHT }

    [System.Serializable]
    private enum StartState { LEFT, STRAIGHT, RIGHT }

    private Tween m_rotateTween;
    private Vector3 m_originalRotation;

    protected override void Awake()
    {
        base.Awake();
        m_originalRotation = m_rotatorPivot.localRotation.eulerAngles;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            if (m_rotatorPivot.localRotation.eulerAngles.y < 10f)
            {
                m_player.ApplySpeedBoostMultiplier(m_speedOverride, m_speedOverrideDuration);
            }
            else
            {
                m_player.Crashed();
            }
        }
    }

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_rotateTween != null)
            m_rotateTween.Kill();

        Vector3 rotateEndPoint = Vector3.zero;
        
        switch(m_directionState)
        {
            case DirectionState.ROTATE_LEFT:
                rotateEndPoint.y = m_startState == StartState.LEFT ? -180f : m_startState == StartState.STRAIGHT ? -90f : 0f;
                break;
            case DirectionState.ROTATE_RIGHT:
                rotateEndPoint.y = m_startState == StartState.LEFT ? 0f : m_startState == StartState.STRAIGHT ? 90f : 180f;
                break;
        }

        m_rotateTween = m_rotatorPivot.DOLocalRotate(rotateEndPoint, m_rotateTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_rotateTween != null)
            m_rotateTween.Kill();

        m_rotateTween = m_rotatorPivot.DOLocalRotate(m_originalRotation, m_rotateTweenDuration);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            switch (m_startState)
            {
                case StartState.LEFT:
                    m_rotatorPivot.localRotation = Quaternion.Euler(0f, -90f, 0f);
                    break;
                case StartState.STRAIGHT:
                    m_rotatorPivot.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case StartState.RIGHT:
                    m_rotatorPivot.localRotation = Quaternion.Euler(0f, 90f, 0f);
                    break;
            }
            
        }
#endif
    }
}
