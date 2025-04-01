using UnityEngine;

public class GameCompleteScreen : ScreenBase
{
    public const string PATH = "Prefabs/UI/Screens/GameCompleteScreen";

    public void OnRestartGamePressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        GameManager.Instance.ResetGameProgressToBeginning();
    }
}
