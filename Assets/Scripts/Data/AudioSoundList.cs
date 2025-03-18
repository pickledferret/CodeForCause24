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
        public AudioClipSettings music1;
        public AudioClipSettings music2;
        public AudioClipSettings music3;
    }


    [System.Serializable]
    public struct SFX
    {
        public AudioClipSettings crowdClapping;
        public AudioClipSettings airHorn;
        public AudioClipSettings boostPad;
        public AudioClipSettings carCrashHorn;
        public AudioClipSettings carCrash;
        public AudioClipSettings carHum;
        public AudioClipSettings endOfGameplayJingle;
        public AudioClipSettings jumpPad;
        public AudioClipSettings levelCompleteTrill;
        public AudioClipSettings rotatorPadUsed;
    }


    [System.Serializable]
    public struct UI
    {
        public AudioClipSettings uiButtonPress;
    }
}