using DG.Tweening;
using UnityEngine;

public class FinalScreen : MonoBehaviour
{
    [SerializeField] private Transform m_heartImage;
    [SerializeField] private Transform m_replayButton;

    private Tween m_heartTween;
    private Tween m_buttonTween;

    void OnEnable()
    {
        if (m_heartImage != null) m_heartTween.Kill();
        if (m_buttonTween!= null) m_buttonTween.Kill();

        m_heartTween = m_heartImage.DOScale(Vector3.one * 1.1f, 0.5f).SetDelay(0.25f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        m_buttonTween = m_replayButton.DOScale(Vector3.one * 1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.endOfGameplayJingle);
    }

    public void OnReplayPressed()
    {
        GameManager.Instance.ResetGame();
    }
}