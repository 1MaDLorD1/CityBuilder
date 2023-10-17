using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour, ISceneLoadHandler<(float, float)>
{
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _soundsVolumeSlider;
    [SerializeField] private AudioSource _musicAudioSource; 
    [SerializeField] private AudioSource _soundsAudioSource;
    [SerializeField] private Button _confirmBtn;
    [SerializeField] private Button _cancelBtn;
    [SerializeField] private GameObject _settingsPanel;

    public AudioSource MusicAudioSource { get => _musicAudioSource; }
    public AudioSource SoundsAudioSource { get => _soundsAudioSource; }

    private void Start()
    {
        _settingsPanel.SetActive(false);
        _musicVolumeSlider.value = MusicAudioSource.volume;
        _soundsVolumeSlider.value = SoundsAudioSource.volume;
        _cancelBtn.onClick.AddListener(OnCancelHandler);
        _confirmBtn.onClick.AddListener(OnConfirmHandler);
    }

    private void OnConfirmHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
        MusicAudioSource.volume = _musicVolumeSlider.value;
        SoundsAudioSource.volume = _soundsVolumeSlider.value;
    }

    private void OnCancelHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
        _settingsPanel.SetActive(false);
    }

    public void OnSceneLoaded((float, float) argument)
    {
        _musicAudioSource.volume = argument.Item1;
        _soundsAudioSource.volume = argument.Item2;
    }
}
