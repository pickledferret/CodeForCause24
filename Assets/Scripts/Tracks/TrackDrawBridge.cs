using DG.Tweening;
using UnityEngine;

public class TrackDrawBridge : TrackBase
{
    [SerializeField] private Transform m_leftBridgePivot;
    [SerializeField] private Transform m_rightBridgePivot;
    [SerializeField] private float m_rotateTweenDuration = 0.75f;

    private Vector3 m_bridgeLeftOriginalRotation;
    private Vector3 m_bridgeRightOriginalRotation;
    private bool m_playerOnBridge;

    private Tween m_bridgeLeftTween;
    private Tween m_bridgeRightTween;

    protected override void Awake()
    {
        base.Awake();

        m_bridgeLeftOriginalRotation = m_leftBridgePivot.localEulerAngles;
        m_bridgeRightOriginalRotation = m_rightBridgePivot.localEulerAngles;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            if (m_leftBridgePivot.localEulerAngles.x < 0.1f)
            {
                m_playerOnBridge = true;
            }
            else
            {
                m_player.Crashed();
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player && m_playerOnBridge)
        {
            m_playerOnBridge = false;
        }
    }

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_bridgeLeftTween != null)
            m_bridgeLeftTween.Kill();

        if (m_bridgeRightTween != null)
            m_bridgeRightTween.Kill();

        m_bridgeLeftTween = m_leftBridgePivot.DOLocalRotate(new Vector3(0f, 0f, 0f), m_rotateTweenDuration);
        m_bridgeRightTween = m_rightBridgePivot.DOLocalRotate(new Vector3(0f, m_bridgeRightOriginalRotation.y, 0f), m_rotateTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_bridgeLeftTween != null)
            m_bridgeLeftTween.Kill();

        if (m_bridgeRightTween != null)
            m_bridgeRightTween.Kill();

        if (m_playerOnBridge)
        {
            m_player.Crashed(3f);
        }

        m_bridgeLeftTween = m_leftBridgePivot.DOLocalRotate(m_bridgeLeftOriginalRotation, m_rotateTweenDuration * 0.75f);
        m_bridgeRightTween = m_rightBridgePivot.DOLocalRotate(m_bridgeRightOriginalRotation, m_rotateTweenDuration * 0.75f);
    }
}
