using UnityEngine;

public class StartScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/StartScreen";

    public void OnStartPressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);
        GameManager.Instance.StartLevel();
    }
}
