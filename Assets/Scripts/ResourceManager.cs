using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField]
    private TaxesManager taxesManager;
    [SerializeField]
    private int startMoneyAmount = 5000;
    [SerializeField]
    private int demolitionPrice = 20;
    [SerializeField]
    private float moneyCalculationInterval = 2;
    [SerializeField]
    private float happinessCalculationInterval = 10;
    MoneyHelper moneyHelper;
    PopulationHelper populationHelper;
    HappinessHelper happinessHelper;
    private BuildingManager buildingManger;
    public UiController uiController;

    public HappinessHelper HappinessHelper {  get { return happinessHelper; } }
    public PopulationHelper PopulationHelper { get { return populationHelper; } }
    public TaxesManager TaxesManager { get { return taxesManager; } }

    public int StartMoneyAmount { get => startMoneyAmount; }
    public float MoneyCalculationInterval { get => moneyCalculationInterval; }
    public float HappinessCalculationInterval { get => happinessCalculationInterval; }

    public int DemolitionPrice => demolitionPrice;

    // Start is called before the first frame update
    void Start()
    {
        moneyHelper = new MoneyHelper(startMoneyAmount, this);
        populationHelper = new PopulationHelper();
        happinessHelper = new HappinessHelper(this);
        UpdateUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this.buildingManger = buildingManager;
        InvokeRepeating("CalculateTownIncome", 0, MoneyCalculationInterval);
        InvokeRepeating("CalculateTownHappiness", 0, HappinessCalculationInterval);
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                moneyHelper.ReduceMoney(amount);
                UpdateUI();
                return true;
            }
            catch (MoneyException)
            {

                ReloadGame();
            }
        }
        return false;
    }

    private void ReloadGame()
    {
        Debug.Log("End the game");
    }

    public bool CanIBuyIt(int amount)
    {
        return moneyHelper.Money >= amount;
    }

    public void CalculateTownIncome()
    {
        try
        {
            moneyHelper.CalculateMoney(buildingManger.GetAllStructures());
            UpdateUI();
        }
        catch (MoneyException)
        {
            ReloadGame();
        }
    }

    public void CalculateTownHappiness()
    {
        happinessHelper.CalculateHappiness(buildingManger.GetAllStructures());
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        moneyHelper.AddMoney(amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        uiController.SetMoneyValue(moneyHelper.Money);
        uiController.SetPopulationValue(populationHelper.Population);
        uiController.SetHappinessValue(happinessHelper.Happiness);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int HowManyStructuresCanIPlace(int placementCost, int numberOfStructures)
    {
        int amount = (int)(moneyHelper.Money / placementCost);
        return amount > numberOfStructures ? numberOfStructures : amount;
    }

    public void AddToPopulation(int value)
    {
        populationHelper.AddToPopulation(value);
        UpdateUI();
    }

    public void ReducePopulation(int value)
    {
        populationHelper.ReducePopulation(value);
        UpdateUI();
    }

    private void OnEnable()
    {
        uiController.PauseButtonPressed += MoneyCalculationIntervalUpdate;
    }

    private void OnDisable()
    {
        uiController.PauseButtonPressed -= MoneyCalculationIntervalUpdate;
        CancelInvoke();
    }

    private void MoneyCalculationIntervalUpdate()
    {
        CancelInvoke();
        if (moneyCalculationInterval == 2) moneyCalculationInterval = 0;
        else
        {
            moneyCalculationInterval = 2;
            InvokeRepeating("CalculateTownIncome", 2, MoneyCalculationInterval);
        }
        if (happinessCalculationInterval == 10) happinessCalculationInterval = 0;
        else
        {
            happinessCalculationInterval = 10;
            InvokeRepeating("CalculateTownHappiness", 10, HappinessCalculationInterval);
        }
    }
}
