using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();
    private WorldManager worldManager;

    private Dictionary<Vector3Int, (string, StructureBaseSO)> allStructuresInfo = new Dictionary<Vector3Int, (string, StructureBaseSO)>();

    public Dictionary<Vector3Int, (string, StructureBaseSO)> AllStructuresInfo { get => allStructuresInfo; set => allStructuresInfo = value; }

    public UnityAction<Vector3Int> StructureDeleted;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PreparePlacementManager(WorldManager worldManager)
    {
        this.worldManager = worldManager;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject CreateGhostStructure(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue = RotationValue.R0)
    {
        GameObject newStructure = PlaceStructureOnTheMap(gridPosition, buildingPrefab, rotationValue);
        Color colorToSet = Color.green;
        ModifyStructurePrefabLook(newStructure, colorToSet);
        return newStructure;
    }

    public GameObject PlaceStructureOnTheMap(Vector3 gridPosition, GameObject buildingPrefab, RotationValue rotationValue)
    {
        GameObject newStructure;
        if (ground == null)
        {
            newStructure = Instantiate(buildingPrefab, new Vector3(0, 0, 0) + gridPosition, Quaternion.identity);
        }
        else
        {
            newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        }
        Vector3 rotation = Vector3.zero;
        switch (rotationValue)
        {
            case RotationValue.R0:
                break;
            case RotationValue.R90:
                rotation = new Vector3(0, 90, 0);
                break;
            case RotationValue.R180:
                rotation = new Vector3(0, 180, 0);
                break;
            case RotationValue.R270:
                rotation = new Vector3(0, 270, 0);
                break;
            default:
                break;
        }
        foreach (Transform child in newStructure.transform)
        {
            child.Rotate(rotation, Space.World);
        }
        return newStructure;
    }

    private void ModifyStructurePrefabLook(GameObject newStructure, Color colorToSet)
    {
        foreach (Transform child in newStructure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (originalMaterials.ContainsKey(child.gameObject) == false)
            {
                originalMaterials.Add(child.gameObject, renderer.materials);
            }
            Material[] materialsToSet = new Material[renderer.materials.Length];
            colorToSet.a = 0.5f;
            for (int i = 0; i < materialsToSet.Length; i++)
            {
                materialsToSet[i] = transparentMaterial;
                materialsToSet[i].color = colorToSet;
            }
            renderer.materials = materialsToSet;
        }
    }

    public void PlaceStructuresOnTheMap(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            if (structure != null)
            {
                worldManager.DestroyNatureAtLocation(structure.transform.position);
                ResetBuildingLook(structure);
            }
        }
        originalMaterials.Clear();
    }

    public void ResetBuildingLook(GameObject structure)
    {
        foreach (Transform child in structure.transform)
        {
            var renderer = child.GetComponent<MeshRenderer>();
            if (originalMaterials.ContainsKey(child.gameObject))
            {
                renderer.materials = originalMaterials[child.gameObject];
            }
        }
    }

    public void DestroyStructures(IEnumerable<GameObject> structureCollection)
    {
        foreach (var structure in structureCollection)
        {
            AllStructuresInfo.Remove(Vector3Int.FloorToInt(structure.transform.position));
            StructureDeleted?.Invoke(Vector3Int.FloorToInt(structure.transform.position));
            DestroySingleStructure(structure);
        }
        originalMaterials.Clear();
    }

    public void DestroySingleStructure(GameObject structure)
    {
        Destroy(structure);
    }


    //public void RemoveBuilding(Vector3 gridPosition, GridStructure grid)
    //{
    //    var structure = grid.GetStructureFromTheGrid(gridPosition);
    //    if (structure != null)
    //    {
    //        Destroy(structure);
    //        grid.RemoveStructureFromTheGrid(gridPosition);
    //    }
    //}

    public void SetBuildingForDemolition(GameObject structureToDemolish)
    {
        Color colorToSet = Color.red;
        ModifyStructurePrefabLook(structureToDemolish, colorToSet);
    }

    public GameObject MoveStructureOnTheMap(Vector3Int positionToPlaceStructure, GameObject gameObjectsToReuse, GameObject prefab)
    {
        gameObjectsToReuse.transform.position = positionToPlaceStructure;
        gameObjectsToReuse.transform.rotation = prefab.transform.rotation;
        for (int i = 0; i < gameObjectsToReuse.transform.childCount; i++)
        {
            gameObjectsToReuse.transform.GetChild(i).rotation = prefab.transform.GetChild(i).rotation;
        }
        return gameObjectsToReuse;
    }
}
