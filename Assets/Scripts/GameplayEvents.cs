using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameplayEvents
{
    /// =====================
    /// Custom Event Triggers
    /// =====================
    
    // Level Start
    public static event Action StartLevelPressed;
    public static void OnStartLevelPressed() { StartLevelPressed?.Invoke(); }

    // Input Pressed
    public static event Action UserInputPressed;
    public static void OnUserInputPressed() { UserInputPressed?.Invoke(); }

    // Input Released
    public static event Action UserInputReleased;
    public static void OnUserInputReleased() { UserInputReleased?.Invoke(); }

    // Tutorial Trigger
    public static event Action<bool> PlayerInsideFTUETrigger;
    public static void OnPlayerInsideFTUETrigger(bool entered) { PlayerInsideFTUETrigger?.Invoke(entered); }

    // UI SCreen Fade
    public static event Action<float, Action, float, float, Action> ScreenFade;
    public static void OnDoScreenFade(float fadeToBlackTime, Action fadeToBlackCallback, float delayBeforeFadeIn, float fadeInTime, Action fadeInCallback)
    {
        ScreenFade?.Invoke(fadeToBlackTime, fadeToBlackCallback, delayBeforeFadeIn, fadeInTime, fadeInCallback);
    }
}