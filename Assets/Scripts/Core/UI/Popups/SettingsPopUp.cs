using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : PopUpBase
{
    public const string PATH = "Prefabs/UI/PopUps/PopUpSettings";

    [SerializeField] private Transform m_content;

    [SerializeField] private Slider m_musicSlider;
    [SerializeField] private Slider m_sfxSlider;

    [SerializeField] private Button m_debugButton;

    private void Awake()
    {
        m_content.localScale = Vector3.zero;
        m_content.DOScale(1f, 0.45f).SetEase(Ease.OutBack);
    }

    private void Start()
    {
        ConfigDebugButton();

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0f);
        m_musicSlider.value = musicVol;

        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0f);
        m_sfxSlider.value = sfxVol;
    }

    private void ConfigDebugButton()
    {
#if UNITY_EDITOR || DEVLOG
        m_debugButton.gameObject.SetActive(true);
        m_debugButton.onClick.AddListener(() => ScreenManager.Instance.PushScreen(DebugScreen.PATH, false));
#else
        m_debugButton.gameObject.SetActive(false);
#endif
    }

    public void OnMusicSliderValChanged(float val)
    {
        AudioManager.Instance.UpdateMusicVolume(val);
    }

    public void OnSFXSliderValChanged(float val)
    {
        AudioManager.Instance.UpdateSFXVolume(val);
    }

    public void OnClosePressed()
    {
        AudioManager audioManager = AudioManager.Instance;
        audioManager.PlayUIAudio(audioManager.AudioSoundList.ui.uiButtonPress);

        m_content.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => ClosePopUp());
    }


}