using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using YG;

public class MainMenuSceneLoader : MonoBehaviour
{
    [SerializeField] private CollectionSO _collectionSO;

    private LevelConfiguration levelConfiguration = new LevelConfiguration();

    public LevelConfiguration LevelConfiguration { get => levelConfiguration; }

    private void Awake()
    {
        if (YandexGame.SDKEnabled == true)
        {
            LoadData();
        }
    }

    private void Start()
    {
        if (YandexGame.SDKEnabled == true)
        {
            LoadData();
        }
    }

    private void OnEnable()
    {
        YandexGame.GetDataEvent += LoadData;
    }

    private void OnDisable()
    {
        YandexGame.GetDataEvent -= LoadData;
    }

    private void LoadData()
    {
        if (YandexGame.savesData.buildingManager != null)
        {
            LevelConfiguration.BuildingManager = YandexGame.savesData.buildingManager;
        }
        if (YandexGame.savesData.moneyHelper != null)
        {
            LevelConfiguration.MoneyHelper = YandexGame.savesData.moneyHelper;
        }
        if (YandexGame.savesData.populationHelper != null)
        {
            LevelConfiguration.PopulationHelper = YandexGame.savesData.populationHelper;
        }
        if (YandexGame.savesData.happinessHelper != null)
        {
            LevelConfiguration.HappinessHelper = YandexGame.savesData.happinessHelper;
        }

        LevelConfiguration.TaxesValue = YandexGame.savesData.taxesValue;

        if (YandexGame.savesData.treesPositions != null)
        {
            LevelConfiguration.TreesPositions = new List<Vector2>(YandexGame.savesData.treesPositions.Count);

            for (int i = 0; i < YandexGame.savesData.treesPositions.Count; i++)
            {
                LevelConfiguration.TreesPositions.Add(new Vector2());
                Vector2 treePosition = LevelConfiguration.TreesPositions[i];
                var position = YandexGame.savesData.treesPositions[i].Split(' ');
                treePosition.x = Convert.ToInt32(position[0]);
                treePosition.y = Convert.ToInt32(position[1]);
                LevelConfiguration.TreesPositions[i] = treePosition;
            }
        }

        if (YandexGame.savesData.treesRemovePositions != null)
        {
            LevelConfiguration.TreesRemovePositions = new List<Vector2>(YandexGame.savesData.treesRemovePositions.Count);

            for (int i = 0; i < YandexGame.savesData.treesRemovePositions.Count; i++)
            {
                LevelConfiguration.TreesRemovePositions.Add(new Vector2());
                Vector2 treePosition = LevelConfiguration.TreesRemovePositions[i];
                var position = YandexGame.savesData.treesRemovePositions[i].Split(' ');
                treePosition.x = Convert.ToInt32(position[0]);
                treePosition.y = Convert.ToInt32(position[1]);
                LevelConfiguration.TreesRemovePositions[i] = treePosition;
            }
        }

        LevelConfiguration.StartAgain = YandexGame.savesData.startAgain;
        LevelConfiguration.QuestsComplete = YandexGame.savesData.questsComplete;

        if (YandexGame.savesData.structureInfoForSave != null)
        {
            LevelConfiguration.AllStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();

            foreach (var structure in YandexGame.savesData.structureInfoForSave)
            {
                Vector3Int postion = new Vector3Int();
                var position = structure.Key.Split(' ');
                postion.x = Convert.ToInt32(position[0]);
                postion.z = Convert.ToInt32(position[1]);
                if (!LevelConfiguration.AllStructuresPositions.ContainsKey(postion))
                {
                    StructureBaseSO structureInfo;

                    if (structure.Value.Item2.structureType == "Road")
                    {
                        structureInfo = new RoadStructureSO();
                        structureInfo.buildingName = structure.Value.Item2.buildingName;
                        structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                        structureInfo.Income = structure.Value.Item2.income;
                        structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                        structureInfo.requireWater = structure.Value.Item2.requireWater;
                        structureInfo.requirePower = structure.Value.Item2.requirePower;
                        structureInfo.structureRange = structure.Value.Item2.structureRange;
                        ((RoadStructureSO)structureInfo).cornerPrefab = _collectionSO.roadStructure.cornerPrefab;
                        ((RoadStructureSO)structureInfo).fourWayPrefab = _collectionSO.roadStructure.fourWayPrefab;
                        ((RoadStructureSO)structureInfo).threeWayPrefab = _collectionSO.roadStructure.threeWayPrefab;
                    }
                    else if (structure.Value.Item2.structureType == "Zone")
                    {
                        structureInfo = new ZoneStructureSO();
                        structureInfo.buildingName = structure.Value.Item2.buildingName;
                        structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                        structureInfo.Income = structure.Value.Item2.income;
                        structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                        structureInfo.requireWater = structure.Value.Item2.requireWater;
                        structureInfo.requirePower = structure.Value.Item2.requirePower;
                        structureInfo.structureRange = structure.Value.Item2.structureRange;
                        ((ZoneStructureSO)structureInfo).maxFacilitySearchRange = structure.Value.Item2.maxFacilitySearchRange;
                        ((ZoneStructureSO)structureInfo).zoneType = structure.Value.Item2.zoneType;
                    }
                    else if (structure.Value.Item2.structureType == "Single")
                    {
                        structureInfo = new SingleFacilitySO();
                        structureInfo.buildingName = structure.Value.Item2.buildingName;
                        structureInfo.upkeepCost = structure.Value.Item2.upkeepCost;
                        structureInfo.Income = structure.Value.Item2.income;
                        structureInfo.requireRoadAccess = structure.Value.Item2.requireRoadAccess;
                        structureInfo.requireWater = structure.Value.Item2.requireWater;
                        structureInfo.requirePower = structure.Value.Item2.requirePower;
                        structureInfo.structureRange = structure.Value.Item2.structureRange;
                        ((SingleFacilitySO)structureInfo).maxCustomers = structure.Value.Item2.maxCustomers;
                        ((SingleFacilitySO)structureInfo).upkeepPerCustomer = structure.Value.Item2.upkeepPerCustomer;
                        ((SingleFacilitySO)structureInfo).facilityType = structure.Value.Item2.facilityType;
                        ((SingleFacilitySO)structureInfo).singleStructureRange = structure.Value.Item2.singleStructureRange;
                    }
                    else
                    {
                        structureInfo = new NullStructureSO();
                    }

                    LevelConfiguration.AllStructuresPositions.Add(postion, (structure.Value.Item1, structureInfo));
                }
            }
        }

        LevelConfiguration.MainMenuSceneLoader = this;
        LevelConfiguration.AgainButtonPressed = YandexGame.savesData.againButtonPressed;
    }
}
