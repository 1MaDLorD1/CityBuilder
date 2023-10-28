
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        //public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int money;
        public int taxesValue = 0;
        public MoneyHelper moneyHelper;
        public PopulationHelper populationHelper;
        public HappinessHelper happinessHelper;
        public float musicVolume;
        public float soundsVolume;
        public BuildingManager buildingManager = null;
        public List<Vector3> roadsPositions = new List<Vector3>();
        public Dictionary<Vector3, string> structuresPositions = new Dictionary<Vector3, string>();
        public List<Vector2> treesPositions = new List<Vector2>();
        public List<Vector2> treesRemovePositions = new List<Vector2>();
        public Dictionary<Vector3Int, (string, StructureBaseSO)> allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
        public Dictionary<Vector3Int, (string, RotationValue, StructureBaseSO)> allRoadsInfo = new Dictionary<Vector3Int, (string, RotationValue, StructureBaseSO)>();
        public bool startAgain = false;
        public bool questsComplete = false;

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
