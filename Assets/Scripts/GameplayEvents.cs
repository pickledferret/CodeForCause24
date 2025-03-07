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
    public static event ScreenFadeDelegate DoScreenFade;
    public static void RaiseDoScreenFade(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback)
    {
        DoScreenFade?.Invoke(fadeToBlackTime, fadeToBlackCallback, delayBeforeFadeIn, fadeInTime, fadeInCallback);
    }

    // Custom Events
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