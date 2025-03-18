using DG.Tweening;
using UnityEngine;

public class TrackExtendingBridge : TrackBase
{
    [SerializeField] private StartState m_startState;
    [SerializeField] private Transform m_bridgePivot;
    [SerializeField] private Transform m_endTrack;
    [SerializeField] private float m_bridgeZOffset = 1f;
    [SerializeField] private float m_bridgeTweenDuration = 0.4f;

    [System.Serializable]
    private enum StartState { RETRACTED, EXTENDED }

    private Tween m_bridgeTween;

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_bridgeTween != null)
        {
            m_bridgeTween.Kill();
        }

        float bridgeScaleEndZ = m_startState == StartState.RETRACTED ? m_bridgeZOffset : 0.25f;
        m_bridgeTween = m_bridgePivot.DOScaleZ(bridgeScaleEndZ, m_bridgeTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_bridgeTween != null)
        {
            m_bridgeTween.Kill();
        }

        float bridgeScaleEndZ = m_startState == StartState.RETRACTED ? 0.25f : m_bridgeZOffset;
        m_bridgeTween = m_bridgePivot.DOScaleZ(bridgeScaleEndZ, m_bridgeTweenDuration);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (m_startState == StartState.RETRACTED)
            {
                m_bridgePivot.localScale = new Vector3(1f, 1f, 0.25f);
            }
            else
            {
                m_bridgePivot.localScale = new Vector3(1f, 1f, m_bridgeZOffset);
            }

            m_endTrack.transform.localPosition = new Vector3(0f, 0f, m_bridgeZOffset + 0.75f);
        }
#endif
    }
}
