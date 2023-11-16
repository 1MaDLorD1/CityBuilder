using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using YG;

public class SceneLoadManager : MonoBehaviour, ISceneLoadHandler<(float, float, LevelConfiguration)>
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _soundsAudioSource;
    [SerializeField] private PlacementManager _placementManager;
    [SerializeField] private MainMenuButton _mainMenuButton;
    [SerializeField] private TaxesManager _taxesManager;
    [SerializeField] private UiController _uiController;

    private BuildingManager _buildingManager;
    private MoneyHelper _moneyHelper;
    private HappinessHelper _happinessHelper;
    private PopulationHelper _populationHelper;
    private int _taxesValue;
    private List<Vector2> _treesPositions;
    private List<Vector2> _treesRemovePositions;
    private bool _startAgain;
    private bool _questsComplete;
    private Dictionary<Vector3Int, (string, StructureBaseSO)> _allStructuresPositions;

    public MainMenuSceneLoader MainMenuSceneLoader { get; set; }

    private bool _againButtonPressed = false;

    private void Awake()
    {
        LoadDataOnAwake();
    }

    private void Start()
    {
        LoadDataOnStart();
    }

    public void OnSceneLoaded((float, float, LevelConfiguration) argument)
    {
        _musicAudioSource.volume = argument.Item1;
        _soundsAudioSource.volume = argument.Item2;

        if (!argument.Item3.AgainButtonPressed)
        {
            if (argument.Item3.QuestsComplete)
            {
                if (argument.Item3.BuildingManager != null)
                {
                    _buildingManager = argument.Item3.BuildingManager;
                }
                if (argument.Item3.MoneyHelper != null)
                {
                    _moneyHelper = argument.Item3.MoneyHelper;
                }
                if (argument.Item3.PopulationHelper != null)
                {
                    _populationHelper = argument.Item3.PopulationHelper;
                }
                if (argument.Item3.HappinessHelper != null)
                {
                    _happinessHelper = argument.Item3.HappinessHelper;
                }
                _taxesValue = argument.Item3.TaxesValue;
                _treesPositions = argument.Item3.TreesPositions;
                _treesRemovePositions = argument.Item3.TreesRemovePositions;
                _startAgain = argument.Item3.StartAgain;
                _questsComplete = true;
                _allStructuresPositions = argument.Item3.AllStructuresPositions;
                MainMenuSceneLoader = argument.Item3.MainMenuSceneLoader;
            }
            else
            {
                _againButtonPressed = false;
                _buildingManager = null;
                _moneyHelper = null;
                _happinessHelper = null;
                _populationHelper = null;
                _taxesValue = 0;
                _treesPositions = new List<Vector2>();
                _treesRemovePositions = new List<Vector2>();
                _startAgain = false;
                _questsComplete = false;
                _allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
            }
        }
        else
        {
            _againButtonPressed = false;
            _buildingManager = null;
            _moneyHelper = null;
            _happinessHelper = null;
            _populationHelper = null;
            _taxesValue = 0;
            _treesPositions = new List<Vector2>();
            _treesRemovePositions = new List<Vector2>();
            _startAgain = false;
            _questsComplete = true;
            _allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
        }

        YandexGame.SaveProgress();
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadDataOnAwake;
        YandexGame.GetDataEvent += LoadDataOnStart;
        _mainMenuButton.MainMenuButtonPressed += SaveData;
        _mainMenuButton.AgainButtonPressed += ReloadData;
        _uiController.ConfirmButtonPressed += SaveData;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= LoadDataOnAwake;
        YandexGame.GetDataEvent -= LoadDataOnStart;
        _mainMenuButton.MainMenuButtonPressed -= SaveData;
        _mainMenuButton.AgainButtonPressed -= ReloadData;
        _uiController.ConfirmButtonPressed -= SaveData;
    }

    private void ReloadData()
    {
        YandexGame.savesData.taxesValue = 0;
        YandexGame.savesData.buildingManager = null;
        YandexGame.savesData.moneyHelper = null;
        YandexGame.savesData.populationHelper = null;
        YandexGame.savesData.happinessHelper = null;
        YandexGame.savesData.treesPositions = new List<string>();
        YandexGame.savesData.treesRemovePositions = new List<string>();
        YandexGame.savesData.startAgain = false;
        YandexGame.savesData.allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
        YandexGame.savesData.questsComplete = true;
        YandexGame.savesData.startFirstTime = false;

        _buildingManager = null;
        _moneyHelper = null;
        _happinessHelper = null;
        _populationHelper = null;
        _taxesValue = 0;
        _treesPositions = new List<Vector2>();
        _treesRemovePositions = new List<Vector2>();
        _startAgain = false;
        _questsComplete = true;
        _allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();

        _againButtonPressed = true;

        YandexGame.savesData.againButtonPressed = _againButtonPressed;
        YandexGame.SaveProgress();
    }

    private void SaveData()
    {
        YandexGame.savesData.taxesValue = _resourceManager.TaxesManager.Taxes;
        YandexGame.savesData.moneyHelper = _resourceManager.MoneyHelper;
        YandexGame.savesData.populationHelper = _resourceManager.PopulationHelper;
        YandexGame.savesData.happinessHelper = _resourceManager.HappinessHelper;

        YandexGame.savesData.treesPositions = new List<string>(_gameManager.worldManager.TreesOnTheMap.Count);

        for (int i = 0; i < _gameManager.worldManager.TreesOnTheMap.Count; i++)
        {
            YandexGame.savesData.treesPositions.Add("");
            var currentPosition = _gameManager.worldManager.TreesOnTheMap[i].x + " " + _gameManager.worldManager.TreesOnTheMap[i].y;
            YandexGame.savesData.treesPositions[i] = currentPosition;
        }

        YandexGame.savesData.treesRemovePositions = new List<string>(_gameManager.worldManager.TreesToRemove.Count);

        for (int i = 0; i < _gameManager.worldManager.TreesToRemove.Count; i++)
        {
            YandexGame.savesData.treesRemovePositions.Add("");
            var currentPosition = _gameManager.worldManager.TreesToRemove[i].x + " " + _gameManager.worldManager.TreesToRemove[i].y;
            YandexGame.savesData.treesRemovePositions[i] = currentPosition;
        }

        YandexGame.savesData.startAgain = _gameManager.StartAgain;

        Dictionary<string, (string, StructureInfo)> structureInfoForSave = new Dictionary<string, (string, StructureInfo)>();

        foreach (var structure in _placementManager.AllStructuresInfo)
        {
            var postion = structure.Key.x + " " + structure.Key.z;
            
            if (!structureInfoForSave.ContainsKey(postion))
            {
                StructureInfo structureInfo = new StructureInfo();
                if (structure.Value.Item2.GetType() == typeof(RoadStructureSO))
                {
                    structureInfo.structureType = "Road";
                    structureInfo.buildingName = structure.Value.Item2.buildingName;
                    structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                    structureInfo.income = structure.Value.Item2.Income;
                    structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                    structureInfo.requireWater = structure.Value.Item2.requireWater;
                    structureInfo.requirePower = structure.Value.Item2.requirePower;
                    structureInfo.structureRange = structure.Value.Item2.structureRange;
                }
                else if (structure.Value.Item2.GetType() == typeof(ZoneStructureSO))
                {
                    structureInfo.structureType = "Zone";
                    structureInfo.buildingName = structure.Value.Item2.buildingName;
                    structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                    structureInfo.income = structure.Value.Item2.Income;
                    structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                    structureInfo.requireWater = structure.Value.Item2.requireWater;
                    structureInfo.requirePower = structure.Value.Item2.requirePower;
                    structureInfo.structureRange = structure.Value.Item2.structureRange;
                    structureInfo.maxFacilitySearchRange = ((ZoneStructureSO)structure.Value.Item2).maxFacilitySearchRange;
                    structureInfo.zoneType = ((ZoneStructureSO)structure.Value.Item2).zoneType;
                }
                else if (structure.Value.Item2.GetType() == typeof(SingleFacilitySO))
                {
                    structureInfo.structureType = "Single";
                    structureInfo.buildingName = structure.Value.Item2.buildingName;
                    structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                    structureInfo.income = structure.Value.Item2.Income;
                    structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                    structureInfo.requireWater = structure.Value.Item2.requireWater;
                    structureInfo.requirePower = structure.Value.Item2.requirePower;
                    structureInfo.structureRange = structure.Value.Item2.structureRange;
                    structureInfo.maxCustomers = ((SingleFacilitySO)structure.Value.Item2).maxCustomers;
                    structureInfo.upkeepPerCustomer = ((SingleFacilitySO)structure.Value.Item2).upkeepPerCustomer;
                    structureInfo.facilityType = ((SingleFacilitySO)structure.Value.Item2).facilityType;
                    structureInfo.singleStructureRange = ((SingleFacilitySO)structure.Value.Item2).singleStructureRange;
                }
                else
                {
                    structureInfo = null;
                }

                structureInfoForSave.Add(postion, (structure.Value.Item1, structureInfo));
            }
        }

        YandexGame.savesData.structureInfoForSave = structureInfoForSave;

        YandexGame.savesData.questsComplete = true;
        YandexGame.savesData.againButtonPressed = _againButtonPressed;
        YandexGame.savesData.startFirstTime = false;
        YandexGame.SaveProgress();
    }

    private void LoadDataOnAwake()
    {
        if (_buildingManager != null)
        {
            _gameManager.BuildingManager = _buildingManager;
            _gameManager.StartAgain = true;
        }
        else
        {
            _gameManager.StartAgain = false;
        }
        if (_moneyHelper != null)
        {
            _resourceManager.MoneyHelper = _moneyHelper;
        }
        if (_populationHelper != null)
        {
            _resourceManager.PopulationHelper = _populationHelper;
        }
        if (_happinessHelper != null)
        {
            _resourceManager.HappinessHelper = _happinessHelper;
        }
        _resourceManager.TaxesManager.Taxes = _taxesValue;
        if (_treesPositions != null)
        {
            _gameManager.worldManager.TreesOnTheMap = _treesPositions;
        }
        if (_treesRemovePositions != null)
        {
            _gameManager.worldManager.TreesToRemove = _treesRemovePositions;
        }
        _uiController.QuestsComplete = _questsComplete;
        _gameManager.StartAgain = _startAgain;
    }

    private void LoadDataOnStart()
    {
        if (_allStructuresPositions != null)
        {
            _placementManager.AllStructuresInfo = _allStructuresPositions;
        }
    }
}
