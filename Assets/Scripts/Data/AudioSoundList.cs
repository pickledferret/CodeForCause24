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
        public AudioClipReference music1;
        public AudioClipReference music2;
        public AudioClipReference music3;
    }


    [System.Serializable]
    public struct SFX
    {
        public AudioClipReference crowdClapping;
        public AudioClipReference airHorn;
        public AudioClipReference boostPad;
        public AudioClipReference carCrashHorn;
        public AudioClipReference carCrash;
        public AudioClipReference carHum;
        public AudioClipReference endOfGameplayJingle;
        public AudioClipReference jumpPad;
        public AudioClipReference levelCompleteTrill;
        public AudioClipReference rotatorPadUsed;
    }


    [System.Serializable]
    public struct UI
    {
        public AudioClipReference uiButtonPress;
    }
}