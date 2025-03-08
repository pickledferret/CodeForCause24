using DG.Tweening;
using UnityEngine;

public class TrackFlipper : TrackBase
{
    [SerializeField] private Transform m_jumpTarget;
    [SerializeField] private float m_jumpHeight = 4f;
    [SerializeField] private float m_jumpDuration = 3f;
    [SerializeField] private Transform m_flipperPivot;
    [SerializeField] private float m_flipperDuration = 0.5f;

    private bool m_playerWasJumped = false;

    private Tween m_flipperTween;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            if (m_flipperTween != null)
            {
                m_player.Crashed();
            }
        }
    }

    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_flipperTween != null)
        {
            m_flipperTween.Kill();
        }

        m_flipperTween = m_flipperPivot.DOLocalRotate(new Vector3(-30f,0,0), m_flipperDuration).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            if (m_playerWasJumped)
            {
                ResetJumpPad();
            }
        });

        if (m_player)
        {
            float playerSplineDistance = m_player.GetCurrentSplineDistancePercentage();
            if (playerSplineDistance < 0.4f || playerSplineDistance > 0.8f)
            {
                float midHeight = m_jumpTarget.position.y - transform.position.y;
                midHeight *= 0.5f;

                Vector3 failedJumpPosition = m_jumpTarget.position;
                failedJumpPosition.y = midHeight;

                m_player.JumpTo(failedJumpPosition, m_jumpHeight / 2f, m_jumpDuration, true);
            }
            else
            {
                m_player.JumpTo(m_jumpTarget.position, m_jumpHeight, m_jumpDuration, false);
            }

            AudioManager audioManager = AudioManager.Instance;
            audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.jumpPad);

            m_playerWasJumped = true;
        }
    }

    protected override void GameplayEvents_UserInputReleased()
    {
        base.GameplayEvents_UserInputReleased();

        ResetJumpPad();
    }

    private void ResetJumpPad()
    {
        if (m_flipperTween != null)
        {
            m_flipperTween.Kill();
        }

        m_flipperTween = m_flipperPivot.DOLocalRotate(new Vector3(10f, 0, 0), 0.35f).OnComplete(() =>
        {
            m_flipperTween = null;
        });

        m_playerWasJumped = false;
    }
}
