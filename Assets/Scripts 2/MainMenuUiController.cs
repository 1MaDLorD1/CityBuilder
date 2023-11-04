using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private VolumeManager _volumeManager;
    [SerializeField] private MainMenuSceneLoader _mainMenuSceneLoader;

    void Start()
    {
        _startGameButton.onClick.AddListener(OnStartGameHandler);
        _settingsButton.onClick.AddListener(OnSettingsHandler);
    }

    private void OnStartGameHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
        var tuple = (_volumeManager.MusicAudioSource.volume, _volumeManager.SoundsAudioSource.volume, _mainMenuSceneLoader.LevelConfiguration);
        BuildingMechanicPrototype.Load(tuple);
    }

    private void OnSettingsHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
        _settingsMenu.SetActive(true);
    }
}
