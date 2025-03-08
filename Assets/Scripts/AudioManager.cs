using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MUSIC_VOL_KEY = "MusicVolume";
    private const string SFX_VOL_KEY = "SFXVolume";
    private const string UI_VOL_KEY = "UIVolume";

    public static AudioManager Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }


    [Header("Audio Sound List")]
    [SerializeField] private AudioSoundList m_audioSoundList;
    public AudioSoundList AudioSoundList => m_audioSoundList;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private AudioMixerSnapshot m_allOnSnapshot;
    [SerializeField] private AudioMixerSnapshot m_allOffSnapshot;
    [SerializeField] private AudioMixerSnapshot m_musicOnlySnapshot;
    [SerializeField] private AudioMixerSnapshot m_sfxOnlySnapshot;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource m_sfxSource;
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioSource m_uiSource;
    [SerializeField] private AudioSource m_playerSource;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(MUSIC_VOL_KEY))
        {
            PlayerPrefs.SetFloat(MUSIC_VOL_KEY, 0.5f);
        }

        if (!PlayerPrefs.HasKey(SFX_VOL_KEY))
        {
            PlayerPrefs.SetFloat(SFX_VOL_KEY, 0.5f);
        }

        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOL_KEY, 0.5f);
        m_audioMixer.SetFloat(MUSIC_VOL_KEY, Mathf.Log10(musicVol) * 20);
        
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOL_KEY, 0.5f);
        m_audioMixer.SetFloat(SFX_VOL_KEY, Mathf.Log10(sfxVol) * 20);
    }

    public void UpdateMusicVolume(float val)
    {
        m_audioMixer.SetFloat(MUSIC_VOL_KEY, Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOL_KEY, val);
    }

    public void UpdateSFXVolume(float val)
    {
        m_audioMixer.SetFloat(SFX_VOL_KEY, Mathf.Log10(val) * 20);
        m_audioMixer.SetFloat(UI_VOL_KEY, Mathf.Log10(val) * 20);
        PlayerPrefs.SetFloat(SFX_VOL_KEY, val);
    }

    public void PlaySFXAudio(AudioClipReference clip)
    {
        m_sfxSource.PlayOneShot(clip.audioClip, clip.volume);
    }

    public void PlayUIAudio(AudioClipReference clip)
    {
        m_uiSource.PlayOneShot(clip.audioClip, clip.volume);
    }

    public void PlayMusicAudio(AudioClipReference clip)
    {
        m_musicSource.clip = clip.audioClip;
        m_musicSource.volume = clip.volume;
        m_musicSource.Play();
    }

    public void PlayPlayerAudio(AudioClipReference clip)
    {
        m_playerSource.clip = clip.audioClip;
        m_playerSource.volume = clip.volume;
        m_playerSource.Play();
        m_playerSource.pitch = 0f;
    }

    public void FadeInPlayerSource()
    {
        m_playerSource.DOPitch(1f, 0.3f);
    }

    public void FadeOutPlayerSource()
    {
        m_playerSource.DOPitch(0f, 0.3f);
    }
}