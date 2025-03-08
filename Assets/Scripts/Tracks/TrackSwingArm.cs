using DG.Tweening;
using UnityEngine;

public class TrackSwingArm : TrackBase
{
    [SerializeField] private DirectionState m_directionState;
    [SerializeField] private TurnState m_turnState;
    [SerializeField] private Transform m_swingArmPivot;
    [SerializeField] private float m_rotateTweenDuration = 1f;

    [Header("Player Override Settings")]
    [SerializeField] private float m_speedOverride = 0.5f;
    [SerializeField] private float m_speedOverrideDuration = 5f;

    [System.Serializable]
    private enum DirectionState { LEFT, RIGHT }

    [System.Serializable]
    private enum TurnState { QUARTER, HALF }

    private Tween m_rotateTween;
    private Quaternion m_originalRotation;

    protected override void Awake()
    {
        base.Awake();
        m_originalRotation = m_swingArmPivot.localRotation;
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

        if (m_rotateTween != null)
            m_rotateTween.Kill();

        Vector3 rotateEndAngles = Vector3.zero;

        switch (m_directionState)
        {
            case DirectionState.LEFT:
                rotateEndAngles.y = m_turnState == TurnState.QUARTER ? 90f : 180f;
                break;
            case DirectionState.RIGHT:
                rotateEndAngles.y = m_turnState == TurnState.QUARTER ? 90f : 180f;
                break;
        }

        m_rotateTween = m_swingArmPivot.DOLocalRotate(rotateEndAngles, m_rotateTweenDuration);

        if (m_player)
        {
            AudioManager audioManager = AudioManager.Instance;
            audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.rotatorPadUsed);
        }
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        if (m_rotateTween != null)
            m_rotateTween.Kill();

        m_rotateTween = m_swingArmPivot.DOLocalRotate(m_originalRotation.eulerAngles, m_rotateTweenDuration * 0.675f);

        if (m_player)
        {
            AudioManager audioManager = AudioManager.Instance;
            audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.rotatorPadUsed);
        }
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            switch (m_directionState)
            {
                case DirectionState.LEFT:
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    break;
                case DirectionState.RIGHT:
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
            }
        }
#endif
    }
}
