using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSoundList", menuName = "AudioSoundList", order = 1)]
public class AudioSoundList : ScriptableObject
{
    [Header("Music")]
    public MUSIC music;

    [Header("SFX")]
    public SFX sfx;

    [Header("UI")]
    public UI ui;

    [System.Serializable]
    public struct MUSIC
    {
        public AudioClipReference music;
    }


    [System.Serializable]
    public struct SFX
    {
        public AudioClipReference sfx;
    }


    [System.Serializable]
    public struct UI
    {
        public AudioClipReference ui;
    }
}