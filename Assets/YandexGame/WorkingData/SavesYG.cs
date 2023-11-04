
using System.Collections.Generic;
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
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int taxesValue = 0;
        public BuildingManager buildingManager;
        public MoneyHelper moneyHelper;
        public PopulationHelper populationHelper;
        public HappinessHelper happinessHelper;
        public List<string> treesPositions = new List<string>();
        public List<string> treesRemovePositions = new List<string>();
        public bool startAgain;
        public Dictionary<Vector3Int, (string, StructureBaseSO)>  allStructuresPositions = new Dictionary<Vector3Int, (string, StructureBaseSO)>();
        public bool questsComplete;
        public bool startFirstTime;
        public bool againButtonPressed;
        public Dictionary<string, (string, StructureInfo)> structureInfoForSave = new Dictionary<string, (string, StructureInfo)>();

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}
