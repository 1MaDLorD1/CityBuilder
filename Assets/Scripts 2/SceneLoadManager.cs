using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SceneLoadManager : MonoBehaviour, ISceneLoadHandler<(float, float)>
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;

    private void Awake()
    {
        LoadDataOnAwake();
    }

    private void Start()
    {
        LoadDataOnStart();
    }

    public void OnSceneLoaded((float, float) argument)
    {
        _musicAudioSource.volume = argument.Item1;
        _soundsAudioSource.volume = argument.Item2;
    }

    private void OnDisable()
    {
        SaveData();
    }

    private void SaveData()
    {
        YandexGame.savesData.buildingManager = _gameManager.BuildingManager;
        YandexGame.savesData.money = _resourceManager.MoneyHelper.Money;
        YandexGame.savesData.happiness = _resourceManager.HappinessHelper.Happiness;
        YandexGame.savesData.roadsPositions = _gameManager.buildingRoadState.RoadsPositions;
        YandexGame.savesData.treesPositions = _gameManager.worldManager.TreesOnTheMap;
    }

    private void LoadDataOnAwake()
    {
        _gameManager.BuildingManager = YandexGame.savesData.buildingManager;
        _gameManager.worldManager.TreesOnTheMap = YandexGame.savesData.treesPositions;
    }

    private void LoadDataOnStart()
    {
        _resourceManager.MoneyHelper.Money = YandexGame.savesData.money;
        _resourceManager.HappinessHelper.Happiness = YandexGame.savesData.happiness;
        _gameManager.buildingRoadState.RoadsPositions = YandexGame.savesData.roadsPositions;
    }
}
