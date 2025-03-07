using UnityEngine;
using DG.Tweening;
using TMPro;
public class GameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text m_levelNumber;

    private void OnEnable()
    {
        int levelNumber = GameManager.Instance.CurrentLevel + 1;
        m_levelNumber.text = $"LEVEL {levelNumber}";
        m_levelNumber.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);
        m_levelNumber.rectTransform.DOAnchorPosY(-69f, 0.675f).SetEase(Ease.OutBack);
    }
}
