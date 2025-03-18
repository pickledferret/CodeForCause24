using DG.Tweening;
using UnityEngine;

public class TrackSeesaw : TrackBase
{
    [SerializeField] private Transform m_seesawPivot;
    [SerializeField] private DirectionState m_directionState;
    [SerializeField] private float m_rotateTweenDuration = 0.75f;

    [System.Serializable]
    private enum DirectionState { NEAR, AWAY };

    private Tween m_seesawRotateTween;

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_seesawRotateTween != null)
        {
            m_seesawRotateTween.Kill();
        }

        m_seesawRotateTween = m_seesawPivot.DOLocalRotate(new Vector3(m_directionState == DirectionState.NEAR ? 30f : -30f, 0f, 0f), m_rotateTweenDuration);
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_seesawRotateTween != null)
        {
            m_seesawRotateTween.Kill();
        }

        m_seesawRotateTween = m_seesawPivot.DOLocalRotate(new Vector3(m_directionState == DirectionState.NEAR ? -30f : 30f, 0f, 0f), m_rotateTweenDuration);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            switch (m_directionState)
            {
                case DirectionState.NEAR:
                    m_seesawPivot.localEulerAngles = new Vector3(-30f, 0f, 0f);
                    break;
                case DirectionState.AWAY:
                    m_seesawPivot.localEulerAngles = new Vector3(30f, 0f, 0f);
                    break;
            }
        }
#endif
    }
}