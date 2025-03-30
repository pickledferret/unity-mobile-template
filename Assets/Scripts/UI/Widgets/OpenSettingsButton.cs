using UnityEngine;

public class OpenSettingsButton : MonoBehaviour
{
    public void OnSettingsButtonPressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        SettingsPopUp settingsScreen = ScreenManager.Instance.ShowPopUp<SettingsPopUp>(SettingsPopUp.PATH);
    }
}
