using DG.Tweening;
using UnityEngine;

public class TrackFloatingPlatform : TrackBase
{
    [SerializeField] private DirectionState m_directionState;
    [SerializeField] private StartState m_startState;
    [SerializeField] private Transform m_platformPivot;
    [SerializeField] private GameObject m_ghostPlatform;
    [SerializeField] private float m_moveTweenDuration = 1f;
    [SerializeField] private float m_platformOffset = 1f;

    [Header("Player Override Settings")]
    [SerializeField] private float m_speedOverride = 0.5f;
    [SerializeField] private float m_speedOverrideDuration = 5f;

    [System.Serializable]
    private enum DirectionState { LEFT, STRAIGHT, RIGHT }

    [System.Serializable]
    private enum StartState { FORWARD, REVERSE }

    private Tween m_moveTween;

    protected override void Awake()
    {
        base.Awake();

        m_ghostPlatform.SetActive(false);
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

        Vector3 moveEndPoint = m_startState == StartState.FORWARD ? GetDirectionOffset() : Vector3.zero;
        m_moveTween = m_platformPivot.DOLocalMove(moveEndPoint, m_moveTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_moveTween != null)
        {
            m_moveTween.Kill();
        }

        Vector3 moveEndPoint = m_startState == StartState.FORWARD ? Vector3.zero : GetDirectionOffset();
        m_moveTween = m_platformPivot.DOLocalMove(moveEndPoint, m_moveTweenDuration * 0.675f);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Vector3 dirOffset = GetDirectionOffset();

            m_platformPivot.transform.localPosition = m_startState == StartState.FORWARD ? Vector3.zero : dirOffset;
            m_ghostPlatform.transform.localPosition = m_startState == StartState.FORWARD ? dirOffset : -dirOffset;
        }
#endif
    }

    private Vector3 GetDirectionOffset()
    {
        return m_directionState switch
        {
            DirectionState.LEFT => new Vector3(-m_platformOffset, 0f, 0f),
            DirectionState.STRAIGHT => new Vector3(0f, 0f, m_platformOffset),
            DirectionState.RIGHT => new Vector3(m_platformOffset, 0f, 0f),
            _ => Vector3.zero,
        };
    }
}
