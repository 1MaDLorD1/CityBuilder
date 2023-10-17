using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button[] mainMenuBtn;
    [SerializeField] private Button[] againBtn;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;

    void Start()
    {
        for (int i = 0; i < mainMenuBtn.Length; i++)
        {
            mainMenuBtn[i].onClick.AddListener(OnMainMenuHandler);
            againBtn[i].onClick.AddListener(OnAgainHandler);
        }
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
        AudioManager.Instance.PlayButtonClickedSound();
        MainMenu.Load((_musicAudioSource.volume, _soundsAudioSource.volume));
    }

    private void OnAgainHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        BuildingMechanicPrototype.Load((_musicAudioSource.volume, _soundsAudioSource.volume));
    }
}
