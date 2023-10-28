using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using YG;

public class SceneLoadManager : MonoBehaviour, ISceneLoadHandler<(float, float)>
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private MainMenuButton _mainMenuButton;
    [SerializeField] private TaxesManager _taxesManager;
    [SerializeField] private UiController _uiController;

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

    private void OnEnable()
    {
        _mainMenuButton.MainMenuButtonPressed += SaveData;
        _mainMenuButton.AgainButtonPressed += ReloadData;
    }

    private void OnDisable()
    {
        _mainMenuButton.MainMenuButtonPressed -= SaveData;
        _mainMenuButton.AgainButtonPressed -= ReloadData;
        //SaveData();
    }

    private void ReloadData()
    {
        YandexGame.savesData.taxesValue = 0;
        YandexGame.savesData.buildingManager = null;
        YandexGame.savesData.moneyHelper = null;
        YandexGame.savesData.populationHelper = null;
        YandexGame.savesData.happinessHelper = null;
        YandexGame.savesData.treesPositions = new List<Vector2>();
        YandexGame.savesData.treesRemovePositions = new List<Vector2>();
        YandexGame.savesData.startAgain = false;
        YandexGame.savesData.allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
        YandexGame.savesData.questsComplete = true;
    }

    private void SaveData()
    {
            YandexGame.savesData.taxesValue = _resourceManager.TaxesManager.Taxes;
            YandexGame.savesData.buildingManager = _gameManager.BuildingManager;
            YandexGame.savesData.moneyHelper = _resourceManager.MoneyHelper;
            YandexGame.savesData.populationHelper = _resourceManager.PopulationHelper;
            YandexGame.savesData.happinessHelper = _resourceManager.HappinessHelper;
            YandexGame.savesData.treesPositions = _gameManager.worldManager.TreesOnTheMap;
            YandexGame.savesData.treesRemovePositions = _gameManager.worldManager.TreesToRemove;
            YandexGame.savesData.startAgain = _gameManager.StartAgain;
            YandexGame.savesData.allStructuresPositions = _placementManager.AllStructuresInfo;
            YandexGame.savesData.questsComplete = true;
    }

    private void LoadDataOnAwake()
    {
        if (YandexGame.savesData.buildingManager != null)
        {
            _gameManager.BuildingManager = YandexGame.savesData.buildingManager;
        }
        if (YandexGame.savesData.moneyHelper != null)
        {
            _resourceManager.MoneyHelper = YandexGame.savesData.moneyHelper;
        }
        if (YandexGame.savesData.populationHelper != null)
        {
            _resourceManager.PopulationHelper = YandexGame.savesData.populationHelper;
        }
        if (YandexGame.savesData.happinessHelper != null)
        {
            _resourceManager.HappinessHelper = YandexGame.savesData.happinessHelper;
        }
        _resourceManager.TaxesManager.Taxes = YandexGame.savesData.taxesValue;
        _gameManager.worldManager.TreesOnTheMap = YandexGame.savesData.treesPositions;
        _gameManager.worldManager.TreesToRemove = YandexGame.savesData.treesRemovePositions;
        _gameManager.StartAgain = YandexGame.savesData.startAgain;
        _uiController.QuestsComplete = YandexGame.savesData.questsComplete;
    }

    private void LoadDataOnStart()
    {
        _placementManager.AllStructuresInfo = YandexGame.savesData.allStructuresPositions;
    }
}
