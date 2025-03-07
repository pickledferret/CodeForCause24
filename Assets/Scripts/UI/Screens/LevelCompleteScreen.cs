using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private List<Transform> m_woopLetters;
    [SerializeField] private float m_letterPopInDuration = 0.5f;
    [SerializeField] private float m_delayBetweenLetters = 0.2f;
    [SerializeField] private Transform m_levelCompleteText;
    [SerializeField] private Transform m_resetButton;

    void Start()
    {
        StartCoroutine(AnimateLetters());
    }

    IEnumerator AnimateLetters()
    {
        foreach (Transform letter in m_woopLetters)
        {
            letter.localScale = new Vector3(letter.localScale.x, 0, letter.localScale.z);
        }
        m_resetButton.localScale = Vector3.zero;
        m_levelCompleteText.localScale = Vector3.zero;


        foreach (Transform letter in m_woopLetters)
        {
            letter.DOScaleY(1, m_letterPopInDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(m_delayBetweenLetters);
        }

        m_levelCompleteText.DOScale(1, m_letterPopInDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(m_delayBetweenLetters * 3f);
        m_resetButton.DOScale(1, m_letterPopInDuration).SetEase(Ease.OutBack);
    }

    public void OnContinuePressed()
    {
        GameManager.Instance.GoToNextLevel();
    }
}
