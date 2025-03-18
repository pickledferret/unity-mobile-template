using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameplayEvents
{
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