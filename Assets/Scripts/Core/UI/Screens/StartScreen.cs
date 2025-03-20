using UnityEngine;

public class StartScreen : ScreenBase
{
    public static string PATH = "Prefabs/UI/Screens/StartScreen";

    public void OnStartPressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);
        GameManager.Instance.LevelStarted();
    }
}
