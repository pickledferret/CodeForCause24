using UnityEngine;
using DG.Tweening;

public class TrackJumper : TrackBase
{
    [SerializeField] private Transform m_jumpTarget;
    [SerializeField] private float m_jumpHeight = 4f;
    [SerializeField] private float m_jumpDuration = 3f;
    [SerializeField] private Transform m_jumpPad;

    private bool m_playerWasJumped = false;

    private Tween m_jumpPadTween;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_player)
        {
            if (m_jumpPadTween != null)
            {
                m_player.Crashed();
            }
        }
    }


    protected override void GameplayEvents_UserInputPressed()
    {
        base.GameplayEvents_UserInputPressed();

        if (m_jumpPadTween != null)
        {
            m_jumpPadTween.Kill();
        }

        m_jumpPadTween = m_jumpPad.DOLocalMoveY(0.1f, 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            if (m_playerWasJumped)
            {
                ResetJumpPad();
            }
        });

        if (m_player)
        {
            if (m_player.GetCurrentSplineDistancePercentage() > 0.65f)
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
        if (m_jumpPadTween != null)
        {
            m_jumpPadTween.Kill();
        }

        m_jumpPadTween = m_jumpPad.DOLocalMoveY(0f, 0.2f).OnComplete(() =>
        {
            m_jumpPadTween = null;
        });

        m_playerWasJumped = false;
    }
}
