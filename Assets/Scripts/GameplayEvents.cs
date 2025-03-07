using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameplayEvents
{
    // Core Delegates
    public delegate void EmptyDelegate();
    public delegate void IntDelegate(int val);
    public delegate void StringDelegate(string val);
    public delegate void BoolDelegate(bool val);
    public delegate void ActionDelegate(Action val);

    // Custom Delegates
    public delegate void ScreenFadeDelegate(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback);


    // Screen Fade Events
    public static event ScreenFadeDelegate ScreenFade;
    public static void OnDoScreenFade(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback)
    {
        ScreenFade?.Invoke(fadeToBlackTime, fadeToBlackCallback, delayBeforeFadeIn, fadeInTime, fadeInCallback);
    }

    // Custom Events
    public static event EmptyDelegate StartLevelPressed;
    public static void OnStartLevelPressed()
    {
        StartLevelPressed?.Invoke();
    }


    public static event EmptyDelegate UserInputPressed;
    public static void OnUserInputPressed()
    {
        UserInputPressed?.Invoke();
    }

    public static event EmptyDelegate UserInputReleased;
    public static void OnUserInputReleased()
    {
        UserInputReleased?.Invoke();
    }
}