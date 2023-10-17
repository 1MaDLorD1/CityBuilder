using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private List<Vector3> treesOnTheMap = new List<Vector3>();
    public GameObject tree;
    public Transform natureParent;
    int width, length;
    GridStructure grid;
    public int radius = 5;

    private const int someCells = 50; // чтобы деревья чуть-чуть заходили за видимую зону при перемещении камеры
    private const int cellSize = 3;

    public GridStructure Grid { get => grid;}
    public List<Vector3> TreesOnTheMap { get => treesOnTheMap; set => treesOnTheMap = value; }

    public void PrepareWorld(int cellSize, int width, int length)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.width = width + someCells;
        this.length = length + someCells;
    }

    public void PrepareTrees()
    {
        TreeGenerator generator = new TreeGenerator(width, length, radius);
        foreach (Vector2 samplePosition in generator.Samples())
        {
            PlaceObjectOnTHeMap(samplePosition, tree);
        }
    }

    public void PrepareTreesAgain()
    {
        foreach (Vector2 samplePosition in TreesOnTheMap.ToList())
        {
            PlaceObjectOnTHeMap(samplePosition, tree);
        }
    }

    private void PlaceObjectOnTHeMap(Vector2 samplePosition, GameObject objectTOCreate)
    {
        var positionInt = Vector2Int.CeilToInt(samplePosition);
        var positionGrid = grid.CalculateGridPosition(new Vector3(positionInt.x, 0, positionInt.y));
        var natureElement = Instantiate(objectTOCreate, positionGrid, Quaternion.identity,natureParent);
        TreesOnTheMap.Add(positionGrid);
        grid.AddNatureToCell(positionGrid, natureElement);
    }

    public void DestroyNatureAtLocation(Vector3 position)
    {
        var elementsTODestroy = grid.GetNaturesObjectsToRemove(position);
        //TreesOnTheMap.FindIndex((Predicate<Vector3>)position);
        foreach (var element in elementsTODestroy)
        {
            Destroy(element);
        }
    }

}
