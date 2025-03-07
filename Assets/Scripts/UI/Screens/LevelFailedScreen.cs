using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailedScreen : MonoBehaviour
{
    [SerializeField] private List<Transform> m_oopsLetters;
    [SerializeField] private float m_letterPopInDuration = 0.5f;
    [SerializeField] private float m_delayBetweenLetters = 0.2f;
    [SerializeField] private Transform m_crashedText;
    [SerializeField] private Transform m_resetButton;

    void Start()
    {
        StartCoroutine(AnimateLetters());
    }

    IEnumerator AnimateLetters()
    {
        foreach (Transform letter in m_oopsLetters)
        {
            letter.localScale = new Vector3(letter.localScale.x, 0, letter.localScale.z);
        }
        m_resetButton.localScale = Vector3.zero;
        m_crashedText.localScale = Vector3.zero;


        foreach (Transform letter in m_oopsLetters)
        {
            letter.DOScaleY(1, m_letterPopInDuration).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(m_delayBetweenLetters);
        }

        m_crashedText.DOScale(1, m_letterPopInDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(m_delayBetweenLetters * 3f);
        m_resetButton.DOScale(1, m_letterPopInDuration).SetEase(Ease.OutBack);
    }

    public void OnResetPressed()
    {
        GameManager.Instance.ResetCurrentLevel();
    }
}
