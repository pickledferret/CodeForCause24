using DG.Tweening;
using UnityEngine;

public class TrackFloatingPlatform : TrackBase
{
    [SerializeField] private DirectionState m_directionState;
    [SerializeField] private Transform m_platformPivot;
    [SerializeField] private GameObject m_ghostPlatform;
    [SerializeField] private float m_moveTweenDuration = 1f;
    [SerializeField] private float m_platformOffset = 1f;

    [Header("Player Override Settings")]
    [SerializeField] private float m_speedOverride = 0.5f;
    [SerializeField] private float m_speedOverrideDuration = 5f;

    [System.Serializable]
    private enum DirectionState { LEFT, STRAIGHT, RIGHT }

    private Tween m_moveTween;
    private Vector3 m_originalPosition;

    protected override void Awake()
    {
        base.Awake();

        m_ghostPlatform.SetActive(false);

        m_originalPosition = m_platformPivot.localPosition;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            m_player.ApplySpeedBoostMultiplier(m_speedOverride, m_speedOverrideDuration);
        }
    }

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_moveTween != null)
        {
            m_moveTween.Kill();
        }

        Vector3 moveEndPoint = Vector3.zero;

        switch (m_directionState)
        {
            case DirectionState.LEFT:
                moveEndPoint.x = -m_platformOffset;
                break;
            case DirectionState.STRAIGHT:
                moveEndPoint.z = m_platformOffset;
                break;
            case DirectionState.RIGHT:
                moveEndPoint.x = m_platformOffset;
                break;
        }

        m_moveTween = m_platformPivot.DOLocalMove(moveEndPoint, m_moveTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_moveTween != null)
        {
            m_moveTween.Kill();
        }

        m_moveTween = m_platformPivot.DOLocalMove(m_originalPosition, m_moveTweenDuration * 0.675f);
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            switch (m_directionState)
            {
                case DirectionState.LEFT:
                    m_ghostPlatform.transform.localPosition = new Vector3(-m_platformOffset, 0f, 0f);
                    break;
                case DirectionState.STRAIGHT:
                    m_ghostPlatform.transform.localPosition = new Vector3(0f, 0f, m_platformOffset);
                    break;
                case DirectionState.RIGHT:
                    m_ghostPlatform.transform.localPosition = new Vector3(m_platformOffset, 0f, 0f);
                    break;
            }

        }
#endif
    }
}
