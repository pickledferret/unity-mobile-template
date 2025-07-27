using DG.Tweening;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [System.Serializable]
    private class MouseControlsTutorial
    {
        public Transform controlsFtuePanel;
    }

    [Header("Mouse Controls FTUE")]
    [SerializeField] private MouseControlsTutorial m_mouseControls;

    private void Awake()
    {
        TutorialEvents.OnTutorialStarted += OnTutorialStarted;
        TutorialEvents.OnTutorialCompleted += OnTutorialCompleted;
    }

    private void OnDestroy()
    {
        TutorialEvents.OnTutorialStarted -= OnTutorialStarted;
        TutorialEvents.OnTutorialCompleted -= OnTutorialCompleted;
    }

    private void OnTutorialStarted(TutorialManager.TutorialMode tutorialMode)
    {
        switch (tutorialMode)
        {
            case TutorialManager.TutorialMode.TUTORIAL_1:
                OnControlsFTUEStarted();
                break;
            case TutorialManager.TutorialMode.TUTORIAL_2:
                break;
            case TutorialManager.TutorialMode.TUTORIAL_3:
                break;
        }
    }

    private void OnTutorialCompleted(TutorialManager.TutorialMode tutorialMode)
    {
        switch (tutorialMode)
        {
            case TutorialManager.TutorialMode.TUTORIAL_1:
                OnControlsFTUECompleted();
                break;
            case TutorialManager.TutorialMode.TUTORIAL_2:
                break;
            case TutorialManager.TutorialMode.TUTORIAL_3:
                break;
        }
    }


    // -----------------
    // CAMERA CONTROLS FTUE
    // -----------------
    private void OnControlsFTUEStarted()
    {
        TutorialEvents.OnLMBPressed += OnLMBPressed;
        TutorialEvents.OnMMBPressed += OnMMBPressed;
        TutorialEvents.OnRMBPressed += OnRMBPressed;

        m_mouseControls.controlsFtuePanel.gameObject.SetActive(true);
        m_mouseControls.controlsFtuePanel.localScale = Vector3.zero;
        m_mouseControls.controlsFtuePanel.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    private void OnControlsFTUECompleted()
    {
        TutorialEvents.OnLMBPressed -= OnLMBPressed;
        TutorialEvents.OnMMBPressed -= OnMMBPressed;
        TutorialEvents.OnRMBPressed -= OnRMBPressed;

        m_mouseControls.controlsFtuePanel.DOScale(0f, 0.25f).OnComplete(() =>
        {
            m_mouseControls.controlsFtuePanel.gameObject.SetActive(false);
        });
    }

    private void OnLMBPressed()
    {
        // Display any visuals for when FTUE - Controls's "LMB" Step is complete.
    }

    private void OnMMBPressed()
    {
        // Display any visuals for when FTUE - Controls's "MMB" Step is complete.
    }

    private void OnRMBPressed()
    {
        // Display any visuals for when FTUE - Controls's "RMB" Step is complete.
    }


    // --------------------------------------
    // ADD TUTORIAL #2 EVENTS AND CODE BELOW
    // --------------------------------------
}