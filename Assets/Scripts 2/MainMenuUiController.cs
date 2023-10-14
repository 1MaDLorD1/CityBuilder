using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IJunior.TypedScenes;

public class MainMenuUiController : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _settingsButton;

    void Start()
    {
        _startGameButton.onClick.AddListener(OnStartGameHandler);
        _settingsButton.onClick.AddListener(OnSettingsHandler);
    }

    private void OnStartGameHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
        BuildingMechanicPrototype.Load();
    }

    private void OnSettingsHandler()
    {
        MainMenuAudioManager.Instance.PlayButtonClickedSound();
    }
}
