using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameplayEvents
{
    // Core Delegates
    public delegate void IntDelegate(int val);
    public delegate void StringDelegate(string val);
    public delegate void BoolDelegate(bool val);
    public delegate void ActionDelegate(Action val);

    // Custom Delegates
    public delegate void ScreenFadeDelegate(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback);

    // Main Menu Events
    public static event Action PlayPressed;
    public static void OnPlayPressed() => PlayPressed?.Invoke();

    // Screen Fade Events
    public static event ScreenFadeDelegate DoScreenFade;
    public static void RaiseDoScreenFade(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback)
    {
        DoScreenFade?.Invoke(fadeToBlackTime, fadeToBlackCallback, delayBeforeFadeIn, fadeInTime, fadeInCallback);
    }

    // Custom Events
    public static event IntDelegate PageUpdated;
    public static void RaisePageUpdated(int pageIndex) => PageUpdated?.Invoke(pageIndex);
}