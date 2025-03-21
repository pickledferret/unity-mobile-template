using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFailedScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/LevelFailedScreen";

    public void OnResetPressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        GameManager.Instance.ResetCurrentLevel();
    }
}
