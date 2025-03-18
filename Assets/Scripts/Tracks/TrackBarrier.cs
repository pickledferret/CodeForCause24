using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TrackBarrier : TrackBase
{
    [SerializeField] private StartState m_startState;
    [SerializeField] private Transform m_barrierPivot;
    [SerializeField] private float m_barrierTweenDuration = 0.4f;

    [System.Serializable]
    private enum StartState { DOWN, UP }

    private Tween m_barrierTween;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            m_player.Crashed();

            Vector3 barrierEndPoint = m_barrierPivot.localEulerAngles;
            barrierEndPoint.z = -90f;
            m_barrierTween = m_barrierPivot.DOLocalRotate(barrierEndPoint, 0.35f).SetEase(Ease.OutBounce);
        }
    }

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_barrierTween != null)
        {
            m_barrierTween.Kill();
        }

        float barrierRotateEndZ = m_startState == StartState.DOWN ? 0f : -90f;
        Vector3 barrierEndPoint = m_barrierPivot.localEulerAngles;
        barrierEndPoint.z = barrierRotateEndZ;
        m_barrierTween = m_barrierPivot.DOLocalRotate(barrierEndPoint, m_barrierTweenDuration).SetEase(Ease.OutBounce);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_barrierTween != null)
        {
            m_barrierTween.Kill();
        }

        float barrierRotateEndZ = m_startState == StartState.DOWN ? -90f : 0f;
        Vector3 barrierEndPoint = m_barrierPivot.localEulerAngles;
        barrierEndPoint.z = barrierRotateEndZ;
        m_barrierTween = m_barrierPivot.DOLocalRotate(barrierEndPoint, m_barrierTweenDuration).SetEase(Ease.OutBounce);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (m_startState == StartState.UP)
            {
                m_barrierPivot.localRotation = Quaternion.identity;
            }
            else
            {
                m_barrierPivot.localRotation = Quaternion.Euler(0, 0, -90f);
            }
        }
#endif
    }
}