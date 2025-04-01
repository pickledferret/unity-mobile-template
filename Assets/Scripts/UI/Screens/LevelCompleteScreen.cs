using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/LevelCompleteScreen";

    public void OnContinuePressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        GameManager.Instance.LoadNextLevel();
    }
}
