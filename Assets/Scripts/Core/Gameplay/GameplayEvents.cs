using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameplayEvents
{
    /// ==============
    /// Core Delegates
    /// ==============
    
    public delegate void EmptyDelegate();
    public delegate void IntDelegate(int val);
    public delegate void StringDelegate(string val);
    public delegate void BoolDelegate(bool val);
    public delegate void ActionDelegate(Action val);

    /// ===================
    /// Core Event Triggers
    /// ===================

    // Level Start
    public static event Action OnLevelStart;
    public static void TriggerLevelStart() { OnLevelStart?.Invoke(); }

    // Level Complete
    public static event Action OnLevelComplete;
    public static void TriggerLevelComplete() { OnLevelComplete?.Invoke(); }

    // Level Failed
    public static event Action OnLevelFailed;
    public static void TriggerLevelFailed() { OnLevelFailed?.Invoke(); }


    /// =====================
    /// Custom Event Triggers
    /// =====================

    // Custom Event
    public static event Action OnNewCustomEvent;
    public static void TriggerCustomEvent() { OnNewCustomEvent?.Invoke(); }
}