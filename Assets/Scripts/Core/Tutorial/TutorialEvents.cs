using UnityEngine;
using System;
using static TutorialManager;

public static partial class TutorialEvents
{
    public static event Action<TutorialMode> OnTutorialStarted;
    public static event Action<TutorialMode> OnTutorialCompleted;

    public static void SendOnTutorialStarted(TutorialMode mode) => OnTutorialStarted?.Invoke(mode);
    public static void SendOnTutorialCompleted(TutorialMode mode) => OnTutorialCompleted?.Invoke(mode);


}