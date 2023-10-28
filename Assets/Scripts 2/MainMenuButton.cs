using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button[] mainMenuBtn;
    [SerializeField] private Button[] againBtn;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;
    [SerializeField] private GameManager _gameManager;

    public UnityAction MainMenuButtonPressed;
    public UnityAction AgainButtonPressed;

    void Start()
    {
        for (int i = 0; i < againBtn.Length; i++)
        {
            againBtn[i].onClick.AddListener(OnAgainHandler);
        }
        mainMenuBtn[0].onClick.AddListener(OnLoseMainMenuHandler);
        mainMenuBtn[1].onClick.AddListener(OnMainMenuHandler);
    }

    private void OnEnable()
    {
        resourceManager.ContinueButtonPressed += OnContinueHandler;
    }

    private void OnDestroy()
    {
        resourceManager.ContinueButtonPressed -= OnContinueHandler;
    }

    private void OnContinueHandler()
    {
        YandexGame.RewVideoShow(0);
    }

    private void OnMainMenuHandler()
    {
        MainMenuButtonPressed?.Invoke();
        AudioManager.Instance.PlayButtonClickedSound();
        MainMenu.Load((_musicAudioSource.volume, _soundsAudioSource.volume));
    }

    private void OnLoseMainMenuHandler()
    {
        AgainButtonPressed?.Invoke();
        AudioManager.Instance.PlayButtonClickedSound();
        MainMenu.Load((_musicAudioSource.volume, _soundsAudioSource.volume));
    }

    private void OnAgainHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _gameManager.StartAgain = false;
        AgainButtonPressed?.Invoke();
        BuildingMechanicPrototype.Load((_musicAudioSource.volume, _soundsAudioSource.volume));
    }
}
