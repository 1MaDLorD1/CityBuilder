using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridStructure
{
    private int cellSize;
    Cell[,] grid;
    private int width, length;
    public GridStructure(int cellSize, int width, int length)
    {
        this.cellSize = cellSize;
        this.width = width;
        this.length = length;
        grid = new Cell[this.width,this.length];
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = new Cell();
            }
        }
    }
    public Vector3 CalculateGridPosition(Vector3 inputPosition)
    {
        int x = Mathf.FloorToInt((float)inputPosition.x / cellSize);
        int z = Mathf.FloorToInt((float)inputPosition.z / cellSize);
        return new Vector3(x * cellSize, 0, z * cellSize);
    }

    internal List<Vector3Int> GetStructurePositionsInRange(Vector3Int gridPosition, int range)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        List<Vector3Int> listToReturn = new List<Vector3Int>();
        if (CheckIndexValidity(cellIndex) == false)
            return listToReturn;
        for (int row = cellIndex.y - range; row <= cellIndex.y + range; row++)
        {
            for (int col = cellIndex.x - range; col <= cellIndex.x + range; col++)
            {
                var tempPosition = new Vector2Int(col, row);
                if (CheckIndexValidity(tempPosition) && Vector2.Distance(cellIndex, tempPosition) <= range)
                {
                    var data = grid[row, col].GetStructureData();
                    if (data != null)
                    {
                        listToReturn.Add(GetGridPositionFromIndex(tempPosition));
                    }
                }
            }
        }
        return listToReturn;
    }

    public bool ArePositionsInRange(Vector3Int gridPosition, Vector3Int structurePositionNearby, int range)
    {
        var distance = Vector2.Distance(CalculateGridIndex(gridPosition), CalculateGridIndex(structurePositionNearby));
        return distance <= range;
    }

    private Vector3Int GetGridPositionFromIndex(Vector2Int tempPosition)
    {
        return new Vector3Int(tempPosition.x * cellSize, 0, tempPosition.y * cellSize);
    }

    private Vector2Int CalculateGridIndex(Vector3 gridPosition)
    {
        return new Vector2Int((int)(gridPosition.x / cellSize), (int)(gridPosition.z / cellSize));
    }

    public bool IsCellTaken(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if(CheckIndexValidity(cellIndex))
            return grid[cellIndex.y, cellIndex.x].IsTaken;
        throw new IndexOutOfRangeException("No index " + cellIndex + " in grid");
    }

    public IEnumerable<StructureBaseSO> GetAllStructures()
    {
        List<StructureBaseSO> structureDataList = new List<StructureBaseSO>();
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                var data = grid[row, col].GetStructureData();
                if (data != null)
                {
                    structureDataList.Add(data);
                }
            }
        }
        return structureDataList;
    }

    public void PlaceStructureOnTheGrid(GameObject structure, Vector3 gridPosition, StructureBaseSO structureData)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if (CheckIndexValidity(cellIndex))
            grid[cellIndex.y, cellIndex.x].SetConstruction(structure, structureData );
    }

    public HashSet<Vector3Int> GetAllPositionsFromTo(Vector3Int minPoint, Vector3Int maxPoint)
    {
        HashSet<Vector3Int> positionsToReturn = new HashSet<Vector3Int>();
        for (int row = minPoint.z; row <= maxPoint.z; row++)
        {
            for (int col = minPoint.x; col <= maxPoint.x; col++)
            {
                Vector3 gridPositon = CalculateGridPosition(new Vector3(col, 0, row));
                positionsToReturn.Add(Vector3Int.FloorToInt(gridPositon));
            }
        }
        return positionsToReturn;
    }

    public GameObject GetStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.y, cellIndex.x].GetStructure();
    }

    public StructureBaseSO GetStructureDataFromTheGrid(Vector3 gridPosition) 
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[cellIndex.y, cellIndex.x].GetStructureData();
    }

    public void RemoveStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        grid[cellIndex.y, cellIndex.x].RemoveStructure();
    }
    private bool CheckIndexValidity(Vector2Int cellIndex)
    {
        if (cellIndex.x >= 0 && cellIndex.x < grid.GetLength(1) && cellIndex.y >= 0 && cellIndex.y < grid.GetLength(0))
            return true;
        return false;
    }

    public Vector3Int? GetPositionOfTheNeighbourIfExists(Vector3 gridPosition, Direction direction)
    {
        Vector3Int? neighbourPosition = Vector3Int.FloorToInt(gridPosition);
        switch (direction)
        {
            case Direction.Up:
                neighbourPosition += new Vector3Int(0, 0, cellSize);
                break;
            case Direction.Right:
                neighbourPosition += new Vector3Int(cellSize, 0, 0);
                break;
            case Direction.Down:
                neighbourPosition += new Vector3Int(0, 0, -cellSize);
                break;
            case Direction.Left:
                neighbourPosition += new Vector3Int(-cellSize, 0, 0);
                break;
        }
        var index = CalculateGridIndex(neighbourPosition.Value);
        if (CheckIndexValidity(index) == false)
        {
            return null;
        }
        return neighbourPosition;
    }

    public IEnumerable<StructureBaseSO> GetStructuresDataInRange(Vector3 gridPosition, int range)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        List<StructureBaseSO> listToReturn = new List<StructureBaseSO>();
        if (CheckIndexValidity(cellIndex) == false)
            return listToReturn;
        for (int row = cellIndex.y-range; row <= cellIndex.y+range; row++)
        {
            for (int col = cellIndex.x-range; col <= cellIndex.x+range; col++)
            {
                var tempPosition = new Vector2Int(col, row);
                if(CheckIndexValidity(tempPosition) && Vector2.Distance(cellIndex, tempPosition) <= range)
                {
                    var data = grid[row, col].GetStructureData();
                    if (data != null)
                    {
                        listToReturn.Add(data);
                    }
                }
            }
        }
        return listToReturn;
    }

    public void AddNatureToCell(Vector3 position, GameObject natureElement)
    {
        var gridPosition = CalculateGridPosition(position);
        var gridIndex = CalculateGridIndex(gridPosition);
        grid[gridIndex.y, gridIndex.x].AddNatureObject(natureElement);
        //var elementsTODestroy = grid[gridIndex.y, gridIndex.x].GetNatureOnThisCell();
        //foreach (var element in elementsTODestroy)
        //{
        //    Debug.Log(element);
        //}
    }

    public List<GameObject> GetNaturesObjectsToRemove(Vector3 position)
    {
        var gridPosition = CalculateGridPosition(position);
        var gridIndex = CalculateGridIndex(gridPosition);
        var elementsTODestroy = grid[gridIndex.y, gridIndex.x].GetNatureOnThisCell();
        //foreach (var element in elementsTODestroy)
        //{
        //    Debug.Log(element);
        //}
        return grid[gridIndex.y, gridIndex.x].GetNatureOnThisCell();
    }

}

public enum Direction
{
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}
