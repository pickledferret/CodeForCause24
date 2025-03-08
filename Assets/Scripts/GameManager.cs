using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private FinalScreen m_finalScreen;
    [Space(10)]
    [SerializeField] private int m_maxLevelSize = 12;
    [Space(10)]
    [SerializeField] private RandomFloorColour m_floorColour;

    [Header("Debug")]
    [SerializeField] private int m_levelOverride = -1;

    public Canvas Canvas => m_canvas;

    private int m_currentLevel = 0;
    public int CurrentLevel => m_currentLevel;

    private string m_currentLoadedLevel;

    private List<AudioClipReference> m_backgroundMusicClips = new List<AudioClipReference>();
    private AudioClipReference m_lastPlayedBackgroundMusicClip;

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
        if (m_levelOverride > -1)
        {
            m_currentLevel = m_levelOverride;
        }
        LoadCurrentLevel();
        BackgroundMusic();
    }

    private void LoadCurrentLevel()
    {
        m_currentLoadedLevel = LEVEL + m_currentLevel;
        SceneManager.LoadScene(m_currentLoadedLevel, LoadSceneMode.Additive);
        m_startScreen.gameObject.SetActive(true);
        m_floorColour.RandomiseColour();
    }
    public void LevelCompleted()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.crowdClapping);
        audioManager.PlaySFXAudio(audioManager.AudioSoundList.sfx.airHorn);

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

            if (m_currentLevel == m_maxLevelSize)
            {
                m_finalScreen.gameObject.SetActive(true);
            }
            else
            {
                LoadCurrentLevel();
            }
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

    public void ResetGame()
    {
        m_currentLevel = 0;

        GameplayEvents.OnDoScreenFade(0.5f, () =>
        {
            m_finalScreen.gameObject.SetActive(false);
            LoadCurrentLevel();
        }, 0.1f, 0.5f, null);
    }

    private void BackgroundMusic()
    {
        AudioManager audioManager = AudioManager.Instance;
        m_backgroundMusicClips.Add(audioManager.AudioSoundList.music.music1);
        m_backgroundMusicClips.Add(audioManager.AudioSoundList.music.music2);
        m_backgroundMusicClips.Add(audioManager.AudioSoundList.music.music3);
        StartCoroutine(PlayRandomBackgroundMusic());
    }

    IEnumerator PlayRandomBackgroundMusic()
    {
        while (true)
        {
            AudioClipReference nextTrack = GetRandomTrack();
            AudioManager.Instance.PlayMusicAudio(nextTrack);
            yield return new WaitForSeconds(nextTrack.audioClip.length);
        }
    }

    private AudioClipReference GetRandomTrack()
    {
        AudioClipReference randomTrack;
        do
        {
            randomTrack = m_backgroundMusicClips[Random.Range(0, m_backgroundMusicClips.Count)];
        }
        while (randomTrack == m_lastPlayedBackgroundMusicClip);

        m_lastPlayedBackgroundMusicClip = randomTrack;

        return randomTrack;
    }
}