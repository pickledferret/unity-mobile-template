using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase
{
    protected Dictionary<string, bool> m_steps = new Dictionary<string, bool>();
    protected Action m_onComplete;

    // -----------------
    // Abstract Methods
    // -----------------
    protected abstract void SetupSteps();
    protected abstract void RegisterEvents();
    protected abstract void UnregisterEvents();

    public virtual void StartTutorial(Action onCompleteCallback)
    {
        m_onComplete = onCompleteCallback;
        SetupSteps();
        RegisterEvents();
    }

    public virtual void CompleteTutorial()
    {
        m_onComplete?.Invoke();
        UnregisterEvents();
    }

    public void CleanUpTutorialForDestroy()
    { 
        UnregisterEvents();
    }

    protected virtual void MarkStepComplete(string step)
    {
        if (!m_steps.ContainsKey(step)) return;

        m_steps[step] = true;
        Devlog.Log("Step " + step + " completed!");

        if (AllStepsCompleted())
        {
            CompleteTutorial();
        }
    }

    private bool AllStepsCompleted()
    {
        foreach (bool step in m_steps.Values)
        {
            if (!step)
                return false;
        }
        return true;
    }
}