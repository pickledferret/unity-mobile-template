using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public enum TutorialMode {NONE, TUTORIAL_1, TUTORIAL_2, TUTORIAL_3}

    public TutorialMode CurrentTutorial => m_currentTutorial;
    private TutorialMode m_currentTutorial = TutorialMode.NONE;

    private Dictionary<TutorialMode, TutorialBase> m_tutorials = new();

    /// ===================
    /// Core Event Triggers
    /// ===================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RegisterTutorial(TutorialMode.TUTORIAL_1, new TutorialControls());
        //..

        StartTutorial(TutorialMode.TUTORIAL_1);
    }

    private void RegisterTutorial(TutorialMode tutorial, TutorialBase tutorialClass)
    {
        m_tutorials.Add(tutorial, tutorialClass);
    }

    public void StartTutorial(TutorialMode tutorial)
    {
        if (m_currentTutorial == TutorialMode.NONE)
        {
            m_currentTutorial = tutorial;
            m_tutorials[m_currentTutorial].StartTutorial(() => CompleteCurrentTutorial());

            Devlog.Log("[TutorialManager]: Tutorial " + m_currentTutorial.ToString() + " started!");

            TutorialEvents.SendOnTutorialStarted(m_currentTutorial);
        }
    }

    private void CompleteCurrentTutorial()
    {
        Devlog.Log("[TutorialManager]: Tutorial " + m_currentTutorial.ToString() + " completed!");
        TutorialEvents.SendOnTutorialCompleted(m_currentTutorial);
        m_currentTutorial = TutorialMode.NONE;
    }

    public void DelayTutorialCompleteCallback(Action callback, float delay)
    {
        StartCoroutine(DelayBeforeCompleteTutorial(callback, delay));
    }

    private IEnumerator DelayBeforeCompleteTutorial(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}