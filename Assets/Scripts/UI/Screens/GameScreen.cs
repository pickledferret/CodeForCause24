using UnityEngine;
using DG.Tweening;
using TMPro;
public class GameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text m_levelNumber;
    [SerializeField] private CanvasGroup m_ftueHandGroup;

    private void OnEnable()
    {
        int levelNumber = GameManager.Instance.CurrentLevel + 1;
        m_levelNumber.text = $"LEVEL {levelNumber}";
        m_levelNumber.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);
        m_levelNumber.rectTransform.DOAnchorPosY(-69f, 0.675f).SetEase(Ease.OutBack);

        m_ftueHandGroup.gameObject.SetActive(false);

        GameplayEvents.PlayerInsideFTUETrigger += GameplayEvents_PlayerInsideFTUETrigger;
    }

    private void OnDisable()
    {
        GameplayEvents.PlayerInsideFTUETrigger -= GameplayEvents_PlayerInsideFTUETrigger;
    }

    private void GameplayEvents_UserInputPressed()
    {
        m_ftueHandGroup.DOFade(0f, 0.2f);
    }

    private void GameplayEvents_UserInputReleased()
    {
        m_ftueHandGroup.DOFade(1f, 0.2f);
    }

    private void GameplayEvents_PlayerInsideFTUETrigger(bool entered)
    {
        if (entered)
        {
            m_ftueHandGroup.gameObject.SetActive(true);
            GameplayEvents.UserInputPressed += GameplayEvents_UserInputPressed;
            GameplayEvents.UserInputReleased += GameplayEvents_UserInputReleased;
            m_ftueHandGroup.alpha = 0f;
            m_ftueHandGroup.DOFade(1f, 0.2f);
        }
        else
        {
            GameplayEvents.UserInputPressed -= GameplayEvents_UserInputPressed;
            GameplayEvents.UserInputReleased -= GameplayEvents_UserInputReleased;
            m_ftueHandGroup.DOFade(0f, 0.2f);
        }
    }
}