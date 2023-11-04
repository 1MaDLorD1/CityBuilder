using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class BuildingManager 
{
    public GridStructure grid;
    public PlacementManager placementManager;
    public StructureRepository structureRepository;
    public IResourceManager resourceManager;
    StructureModificationHelper helper;
    RoadPlacementModificationHelper roadHelper;
    SingleStructurePlacementHelper structureHelper;

    public Dictionary<Vector3, (string, string)> structureInfoForSave = new Dictionary<Vector3, (string, string)>();

    private Dictionary<Vector3Int, (GameObject, StructureBaseSO, RotationValue)> fullStructuresInfo = new Dictionary<Vector3Int, (GameObject, StructureBaseSO, RotationValue)>();

    private List<GameObject> structuresObjects = new List<GameObject>();

    private Dictionary<Vector3Int, GameObject> roadsToBeModified = new Dictionary<Vector3Int, GameObject>();

    public StructureModificationHelper Helper { get => helper; set => helper = value; }

    public BuildingManager(GridStructure grid,PlacementManager placementManager, StructureRepository structureRepository, IResourceManager resourceManager)
    {
        this.grid = grid;
        this.placementManager = placementManager;
        this.structureRepository = structureRepository;
        this.resourceManager = resourceManager;
        StructureModificationFactory.PrepareFactory(structureRepository, grid, placementManager, resourceManager);
        placementManager.StructureDeleted += OnStructureDeleted;
    }

    ~BuildingManager()
    {
        placementManager.StructureDeleted -= OnStructureDeleted;
        ((StructureModificationHelper)helper).StructureAdded -= OnStructureAdded;
    }

    private void OnStructureAdded()
    {
        foreach (var structure in placementManager.AllStructuresInfo) 
        {
            if (!structureInfoForSave.ContainsKey(structure.Key))
            {
                string structureInfo;
                if (structure.Value.Item2.GetType() == typeof(RoadStructureSO))
                {
                    structureInfo = "Road";
                }
                else if (structure.Value.Item2.GetType() == typeof(ZoneStructureSO))
                {
                    structureInfo = "Zone";
                }
                else if (structure.Value.Item2.GetType() == typeof(SingleFacilitySO))
                {
                    structureInfo = "Single";
                }
                else
                {
                    structureInfo = "Null";
                }

                structureInfoForSave.Add(structure.Key, (structure.Value.Item1, structureInfo));
            }
        }
    }

    private void OnStructureDeleted(Vector3Int position)
    {
        if (fullStructuresInfo.TryGetValue(position, out var road) && road.Item2.GetType() == typeof(RoadStructureSO))
        {
            Debug.Log("kek");
        }
        fullStructuresInfo.Remove(position);
        if (roadsToBeModified.ContainsKey(position))
        {
            roadsToBeModified.Remove(position);
        }
    }

    public void ConfirmModificationsOnStart()
    {
        StructureModificationFactory.PrepareFactory(structureRepository, grid, placementManager, resourceManager);
        placementManager.StructureDeleted += OnStructureDeleted;

        foreach (var structure in placementManager.AllStructuresInfo)
        {
            StructureType structureType;

            if (structure.Value.Item2.GetType() == typeof(ZoneStructureSO))
            {
                structureType = StructureType.Zone;
            }
            else if (structure.Value.Item2.GetType() == typeof(RoadStructureSO))
            {
                structureType = StructureType.Road;
            }
            else if (structure.Value.Item2.GetType() == typeof(SingleFacilitySO))
            {
                structureType = StructureType.SingleStructure;
            }
            else
            {
                structureType = StructureType.None;
            }

            if (!fullStructuresInfo.ContainsKey(structure.Key))
            {
                GameObject prefab = structureRepository.GetBuildingPrefabByName(structure.Value.Item1, structureType);
                structure.Value.Item2.prefab = prefab;
                fullStructuresInfo.Add(structure.Key, (prefab, structure.Value.Item2, RotationValue.R0));
            }
        }

        foreach (var structure in fullStructuresInfo)
        {
            structuresObjects.Add(structure.Value.Item1);

            if(structure.Value.Item2.buildingName == "Дорога" && !roadsToBeModified.ContainsKey(structure.Key))
            {
                roadsToBeModified.Add(structure.Key, structure.Value.Item1);
            }
        }

        placementManager.PlaceStructuresOnTheMap(structuresObjects);
        //Type structureType = structureData.GetType();
        foreach (var keyValuePair in fullStructuresInfo)
        {
            if (keyValuePair.Value.Item2.buildingName == "Дорога")
            {
                var roadStructure = RoadManager.GetCorrectRoadPrefab(keyValuePair.Key, keyValuePair.Value.Item2, roadsToBeModified, grid);
                var newStructure = placementManager.PlaceStructureOnTheMap(keyValuePair.Key, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation);
                grid.PlaceStructureOnTheGrid(newStructure, keyValuePair.Key, GameObject.Instantiate(keyValuePair.Value.Item2));
            }
            else
            {
                var newStructure = placementManager.PlaceStructureOnTheMap(keyValuePair.Key, keyValuePair.Value.Item1, RotationValue.R0);
                grid.PlaceStructureOnTheGrid(newStructure, keyValuePair.Key, GameObject.Instantiate(keyValuePair.Value.Item2));
            }

            
            StructureEconomyManager.CreateStructureLogic(keyValuePair.Value.Item2.GetType(), keyValuePair.Key, grid);
        }

        fullStructuresInfo = new Dictionary<Vector3Int, (GameObject, StructureBaseSO, RotationValue)>();
    }

    public void PrepareBuildingManager(Type classType)
    {
        Helper = StructureModificationFactory.GetHelper(classType);
        ((StructureModificationHelper)helper).StructureAdded += OnStructureAdded;
    }

    public void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        Helper.PrepareStructureForPlacement(inputPosition, structureName, structureType);
    }

    public void ConfirmModification()
    {
        Helper.ConfirmModifications();
    }

    public void CancleModification()
    {
        Helper.CancleModifications();
    }

    public IEnumerable<StructureBaseSO> GetAllStructures()
    {
        return grid.GetAllStructures();
    }

    public void PrepareStructureForDemolitionAt(Vector3 inputPosition)
    {
        Helper.PrepareStructureForPlacement(inputPosition, "", StructureType.None);
    }


    public GameObject CheckForStructureInGrid(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition))
        {
            return grid.GetStructureFromTheGrid(gridPosition);

        }
        return null;
    }

    public GameObject CheckForStructureInDictionary(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        GameObject structureToReturn = null;
        structureToReturn = Helper.AccessStructureInDictionary(gridPosition);
        if (structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = Helper.AccessStructureInDictionary(gridPosition);
        return structureToReturn;
    }

    public void StopContinuousPlacement()
    {
        Helper.StopContinuousPlacement();
    }

    public StructureBaseSO GetStructureDataFromPosition(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition))
        {
            return grid.GetStructureDataFromTheGrid(inputPosition);

        }
        return null;
    }
}


