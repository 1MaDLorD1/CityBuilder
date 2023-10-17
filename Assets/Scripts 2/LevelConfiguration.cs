using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Config")]
public class LevelConfiguration : ScriptableObject
{
    [SerializeField] private int _money;
    [SerializeField] private int _happiness;
    [SerializeField] private int _musicVolume;
    [SerializeField] private int _soundsVolume;
    [SerializeField] GameManager _gameManager;

    private BuildingManager _buildingManager;

}
