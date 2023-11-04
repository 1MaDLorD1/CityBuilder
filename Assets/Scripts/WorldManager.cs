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
    private List<Vector2> treesOnTheMap = new List<Vector2>();
    private List<Vector2> treesToRemove = new List<Vector2>();
    public GameObject tree;
    public Transform natureParent;
    int width, length;
    GridStructure grid;
    public int radius = 5;

    private const int someCells = 50; // чтобы деревья чуть-чуть заходили за видимую зону при перемещении камеры
    private const int cellSize = 3;

    public GridStructure Grid { get => grid;}
    public List<Vector2> TreesOnTheMap { get => treesOnTheMap; set => treesOnTheMap = value; }
    public List<Vector2> TreesToRemove { get => treesToRemove; set => treesToRemove = value; }

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
            if (!TreesToRemove.Contains(samplePosition))
            {
                PlaceObjectOnTHeMapAgain(samplePosition, tree);
            }
        }
    }

    private void PlaceObjectOnTHeMap(Vector2 samplePosition, GameObject objectTOCreate)
    {
        var positionInt = Vector2Int.CeilToInt(samplePosition);
        var positionGrid = grid.CalculateGridPosition(new Vector3(positionInt.x, 0, positionInt.y));
        var natureElement = Instantiate(objectTOCreate, positionGrid, Quaternion.identity,natureParent);
        TreesOnTheMap.Add(new Vector2(positionGrid.x, positionGrid.z));
        grid.AddNatureToCell(positionGrid, natureElement);
    }

    private void PlaceObjectOnTHeMapAgain(Vector2 samplePosition, GameObject objectTOCreate)
    {
        var positionInt = Vector2Int.CeilToInt(samplePosition);
        var positionGrid = grid.CalculateGridPosition(new Vector3(positionInt.x, 0, positionInt.y));
        var natureElement = Instantiate(objectTOCreate, positionGrid, Quaternion.identity, natureParent);
        grid.AddNatureToCell(positionGrid, natureElement);
    }

    public void DestroyNatureAtLocation(Vector3 position)
    {
        var elementsTODestroy = grid.GetNaturesObjectsToRemove(position);
        if(!TreesToRemove.Contains(new Vector2(position.x, position.z)))
            TreesToRemove.Add(new Vector2(position.x, position.z));
        foreach (var element in elementsTODestroy)
        {
            Destroy(element);
        }
    }

}
