using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int m_currentLevel = 0;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start()
    {
    }

    public void EnterLevel()
    {
        SceneManager.LoadScene("Workshop", LoadSceneMode.Additive);
    }

    public void LevelCompleted()
    {
        //AudioManager audioManager = AudioManager.Instance;
        //audioManager.PlaySFXAudio(audioManager.AudioSoundList.ui.onFinishPressed);

        m_currentLevel++;

        GameplayEvents.RaiseDoScreenFade(0.5f, () =>
        {
            // On Screen Faded To Black.
            SceneManager.UnloadSceneAsync("Workshop");
        }, 0.1f, 0.5f, null);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene '{scene.name}' loaded.");
        if (scene.name != "MainScene")
        {
    		// Start Gameplay...
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"Scene '{scene.name}' unloaded.");
        if (scene.name == "Workshop")
        {
        }
    }
}