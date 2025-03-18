using DG.Tweening;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private Transform m_text;

    private Sequence m_textScale;

    private void OnEnable()
    {
        // Pulse Scale the Tap To Start text.
        m_textScale = DOTween.Sequence();
        m_textScale.Append(m_text.DOScale(1.1f, 0.5f));
        m_textScale.Append(m_text.DOScale(1f, 0.5f));
        m_textScale.SetLoops(-1);
        m_textScale.Play();
    }

    private void OnDisable()
    {
        m_textScale.Kill();
        m_text.localScale = Vector3.one;
    }

    public void OnStartPressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
