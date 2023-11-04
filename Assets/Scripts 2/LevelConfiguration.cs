using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfiguration
{
    public BuildingManager BuildingManager { get; set; }
    public MoneyHelper MoneyHelper { get; set; }
    public HappinessHelper HappinessHelper { get; set; }
    public PopulationHelper PopulationHelper { get; set; }
    public int TaxesValue { get; set; }
    public List<Vector2> TreesPositions { get; set; }
    public List<Vector2> TreesRemovePositions { get; set; }
    public bool StartAgain { get; set; }
    public bool QuestsComplete { get; set; }
    public Dictionary<Vector3Int, (string, StructureBaseSO)> AllStructuresPositions { get; set; }
    public MainMenuSceneLoader MainMenuSceneLoader { get; set; }
    public bool AgainButtonPressed { get; set; }
    public bool StartFirstTime {  get; set; }
}
