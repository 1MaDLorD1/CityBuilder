using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField]
    private int moneyAddingAfterContinue = 5000;
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

    public MoneyHelper MoneyHelper { get => moneyHelper; }

    public UnityAction ContinueButtonPressed;

    // Start is called before the first frame update
    void Awake()
    {
        moneyHelper = new MoneyHelper(startMoneyAmount, this);
        populationHelper = new PopulationHelper();
        happinessHelper = new HappinessHelper(this);
        UpdateUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this.buildingManger = buildingManager;
        moneyCalculationInterval = 2;
        happinessCalculationInterval = 10;
        InvokeRepeating("CalculateTownIncome", 0, MoneyCalculationInterval);
        InvokeRepeating("CalculateTownHappiness", 0, HappinessCalculationInterval);
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                MoneyHelper.ReduceMoney(amount);
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
        MoneyCalculationIntervalUpdate();
        uiController.loseMenuPanel.SetActive(true);
    }

    public bool CanIBuyIt(int amount)
    {
        return MoneyHelper.Money >= amount;
    }

    public void CalculateTownIncome()
    {
        try
        {
            MoneyHelper.CalculateMoney(buildingManger.GetAllStructures());
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

        if (happinessHelper.Happiness < 0)
        {
            uiController.HappinessImage.sprite = uiController.LowHappinessIcon;
            uiController.HappinessImage.color = uiController.LowHappinesscColor;
        }
        else
        {
            uiController.HappinessImage.sprite = uiController.HighHappinessIcon;
            uiController.HappinessImage.color = uiController.HighHappinesscColor;
        }

        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        MoneyHelper.AddMoney(amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        uiController.SetMoneyValue(MoneyHelper.Money);
        uiController.SetPopulationValue(populationHelper.Population);
        uiController.SetHappinessValue(happinessHelper.Happiness);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int HowManyStructuresCanIPlace(int placementCost, int numberOfStructures)
    {
        int amount = (int)(MoneyHelper.Money / placementCost);
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
        uiController.ContinueButtonPressed += OnContinueButtonPressed;
        uiController.MenuButtonPressed += OnMenuButtonPressed;
        uiController.ContinueMenuButtonPressed += OnContinueMenuButtonPressed;
    }

    private void OnDisable()
    {
        uiController.PauseButtonPressed -= MoneyCalculationIntervalUpdate;
        uiController.ContinueButtonPressed -= OnContinueButtonPressed;
        uiController.MenuButtonPressed -= OnMenuButtonPressed;
        uiController.ContinueMenuButtonPressed -= OnContinueMenuButtonPressed;
        CancelInvoke();
    }

    private void OnMenuButtonPressed()
    {
        MoneyCalculationIntervalUpdate();
    }

    private void OnContinueMenuButtonPressed()
    {
        MoneyCalculationIntervalUpdate();
    }

    private void OnContinueButtonPressed()
    {
        ContinueButtonPressed?.Invoke();
        AddMoney(moneyAddingAfterContinue);
        MoneyCalculationIntervalUpdate();
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
