using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string LEVEL = "Level";

    public static GameManager Instance { get; private set; }

    [SerializeField] private Canvas m_canvas;
    [SerializeField] private StartScreen m_startScreen;
    [SerializeField] private GameScreen m_gameScreen;
    [SerializeField] private LevelCompleteScreen m_levelCompleteScreen;
    [SerializeField] private LevelFailedScreen m_levelFailedScreen;
    public Canvas Canvas => m_canvas;

    private int m_currentLevel = 0;
    public int CurrentLevel => m_currentLevel;

    private string m_currentLoadedLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadCurrentLevel();
    }

    private void LoadCurrentLevel()
    {
        m_currentLoadedLevel = LEVEL + m_currentLevel;
        SceneManager.LoadScene(m_currentLoadedLevel, LoadSceneMode.Additive);

        m_startScreen.gameObject.SetActive(true);
    }

    public void LevelCompleted()
    {
        //AudioManager audioManager = AudioManager.Instance;
        //audioManager.PlaySFXAudio(audioManager.AudioSoundList.ui.onFinishPressed);

        m_currentLevel++;

        m_gameScreen.gameObject.SetActive(false);
        m_levelCompleteScreen.gameObject.SetActive(true);
    }

    public void LevelFailed()
    {
        m_gameScreen.gameObject.SetActive(false);
        m_levelFailedScreen.gameObject.SetActive(true);
    }

    public void GoToNextLevel()
    {
        GameplayEvents.OnDoScreenFade(0.5f, () =>
        {
            // On Screen Faded To Black.
            m_levelCompleteScreen.gameObject.SetActive(false);
            SceneManager.UnloadSceneAsync(m_currentLoadedLevel);
            LoadCurrentLevel();
        }, 0.1f, 0.5f, null);
    }

    public void ResetCurrentLevel()
    {
        GameplayEvents.OnDoScreenFade(0.5f, () =>
        {
            // On Screen Faded To Black.
            m_levelFailedScreen.gameObject.SetActive(false);
            SceneManager.UnloadSceneAsync(m_currentLoadedLevel);
            LoadCurrentLevel();
        }, 0.1f, 0.5f, null);
    }

    public void StartGame()
    {
        m_startScreen.gameObject.SetActive(false);
        m_gameScreen.gameObject.SetActive(true);
        GameplayEvents.OnStartLevelPressed();
    }
}