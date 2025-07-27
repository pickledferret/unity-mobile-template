using System;
using UnityEngine;

public class TutorialControls : TutorialBase
{
    private const string LEFT_MOUSE = "LMB";
    private const string MIDDLE_MOUSE = "MMB";
    private const string RIGHT_MOUSE = "RMB";

    private Action onLMBPressed;
    private Action onMMBPressed;
    private Action onRMBPressed;

    protected override void SetupSteps()
    {
        m_steps[LEFT_MOUSE] = false;
        m_steps[MIDDLE_MOUSE] = false;
        m_steps[RIGHT_MOUSE] = false;
    }

    protected override void RegisterEvents()
    {
        onLMBPressed = () => MarkStepComplete(LEFT_MOUSE);
        onMMBPressed = () => MarkStepComplete(MIDDLE_MOUSE);
        onRMBPressed = () => MarkStepComplete(RIGHT_MOUSE);

        TutorialEvents.OnLMBPressed += onLMBPressed;
        TutorialEvents.OnMMBPressed += onMMBPressed;
        TutorialEvents.OnRMBPressed += onRMBPressed;
    }

    protected override void UnregisterEvents()
    {
        TutorialEvents.OnLMBPressed -= onLMBPressed;
        TutorialEvents.OnMMBPressed -= onMMBPressed;
        TutorialEvents.OnRMBPressed -= onRMBPressed;
    }

    public override void StartTutorial(Action onCompleteCallback)
    {
        base.StartTutorial(onCompleteCallback);
    }

    public override void CompleteTutorial()
    {
        /// Optional - Delay before calling base.CompleteTutorial().
        /// Useful for if a tutorial displays something to the user on completion. Else, it may be destroyed immediately after completion.
        Devlog.Log("Hit Child Class");
        TutorialManager.Instance.DelayTutorialCompleteCallback(base.CompleteTutorial, 1f);
    }

    protected override void MarkStepComplete(string step)
    {
        base.MarkStepComplete(step);
    }
}
