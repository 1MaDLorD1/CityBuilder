using NSubstitute;
using NSubstitute.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private Sprite _pauseIcon;
    [SerializeField] private Sprite _resumeIcon;
    [SerializeField] private Sprite _lowHappinessIcon;
    [SerializeField] private Sprite _highHappinessIcon;
    [SerializeField] private Image _happinessImage;
    [SerializeField] private Color _highHappinesscColor;
    [SerializeField] private Color _lowHappinesscColor;
    [SerializeField] private GameObject[] _nubiksQuests;

    public Sprite LowHappinessIcon { get => _lowHappinessIcon; }
    public Sprite HighHappinessIcon { get => _highHappinessIcon; }
    public Image HappinessImage { get => _happinessImage; set => _happinessImage = value; }
    public Color HighHappinesscColor { get => _highHappinesscColor; }
    public Color LowHappinesscColor { get => _lowHappinesscColor; }

    private Action<string> OnBuildAreaHandler;
    private Action<string> OnBuildSingleStructureHandler;
    private Action<string> OnBuildRoadHandler;

    private Action OnCancleActionHandler;
    private Action OnConfirmActionHandler;
    private Action OnDemolishActionHandler;

    public StructureRepository structureRepository;
    public Button buildResidentialAreaBtn;
    public Button cancleActionBtn;
    public Button confirmActionBtn;
    public GameObject cancleActionPanel;

    public GameObject buildingMenuPanel;
    public Button openBuildMenuBtn;
    public Button pauseBtn;
    public Button demolishBtn;

    public GameObject loseMenuPanel;
    public Button continueBtn;

    public GameObject menuPanel;
    public Button menuBtn;
    public Button continueMenuBtn;

    public GameObject taxesMenuPanel;
    public Button closeTaxesMenuBtn;
    public Button openTaxesMenuBtn;

    public GameObject zonesPanel;
    public GameObject facilitiesPanel;
    public GameObject roadsPanel;
    public Button closeBuildMenuBtn;

    public GameObject buildButtonPrefab;

    public TextMeshProUGUI moneyValue;
    public TextMeshProUGUI populationValue;
    public TextMeshProUGUI happinessValue;

    public UIStructureInfoPanelHelper structurePanelHelper;

    public UnityAction PauseButtonPressed;
    public UnityAction ContinueButtonPressed;
    public UnityAction MenuButtonPressed;
    public UnityAction ContinueMenuButtonPressed;

    private bool _pause = false;

    public bool QuestsComplete = false;

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        cancleActionPanel.SetActive(false);
        buildingMenuPanel.SetActive(false);
        taxesMenuPanel.SetActive(false);
        loseMenuPanel.SetActive(false);
        menuPanel.SetActive(false);
        //buildResidentialAreaBtn.onClick.AddListener(OnBuildAreaCallback);
        cancleActionBtn.onClick.AddListener(OnCancleActionCallback);
        confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);
        openBuildMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        openTaxesMenuBtn.onClick.AddListener(OnOpenTaxesMenu);
        demolishBtn.onClick.AddListener(OnDemolishHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);
        closeTaxesMenuBtn.onClick.AddListener(OnCloseTaxesMenuHandler);
        pauseBtn.onClick.AddListener(OnPauseHandler);
        continueBtn.onClick.AddListener(OnContinueHandler);
        menuBtn.onClick.AddListener(OnMenuHandler);
        continueMenuBtn.onClick.AddListener(OnContinueMenuHandler);

        if (!QuestsComplete)
        {
            cancleActionBtn.onClick.RemoveAllListeners();
            confirmActionBtn.onClick.RemoveAllListeners();
            openBuildMenuBtn.onClick.RemoveAllListeners();
            openTaxesMenuBtn.onClick.RemoveAllListeners();
            demolishBtn.onClick.RemoveAllListeners();
            closeBuildMenuBtn.onClick.RemoveAllListeners();
            closeTaxesMenuBtn.onClick.RemoveAllListeners();
            pauseBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.RemoveAllListeners();
            menuBtn.onClick.RemoveAllListeners();
            continueMenuBtn.onClick.RemoveAllListeners();
            _nubiksQuests[0].SetActive(true);
            _nubiksQuests[0].GetComponent<Button>().onClick.AddListener(OnFirstQuestHandler);
            _nubiksQuests[1].GetComponent<Button>().onClick.AddListener(OnQuestSecondHandler);
            _nubiksQuests[2].GetComponent<Button>().onClick.AddListener(OnThirdQuestHandler);
            _nubiksQuests[6].GetComponent<Button>().onClick.AddListener(OnSeventhQuestHandler);
            _nubiksQuests[7].GetComponent<Button>().onClick.AddListener(OnEightQuestHandler);
            _nubiksQuests[8].GetComponent<Button>().onClick.AddListener(OnNineQuestHandler);
            _nubiksQuests[9].GetComponent<Button>().onClick.AddListener(OnTenQuestHandler);
            _nubiksQuests[10].GetComponent<Button>().onClick.AddListener(OnElevenQuestHandler);
        }
    }

    private void OnFirstQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[0].SetActive(false);
        _nubiksQuests[1].SetActive(true);
    }

    private void OnQuestSecondHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[1].SetActive(false);
        _nubiksQuests[2].SetActive(true);
    }

    private void OnThirdQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[2].SetActive(false);
        _nubiksQuests[3].SetActive(true);
        openBuildMenuBtn.onClick.AddListener(OnOpenBuildMenu);
    }

    private void OnSeventhQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[6].SetActive(false);
        _nubiksQuests[7].SetActive(true);
        demolishBtn.onClick.AddListener(OnDemolishHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);
    }

    private void OnEightQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[7].SetActive(false);
        _nubiksQuests[8].SetActive(true);
        pauseBtn.onClick.AddListener(OnPauseHandler);
        openTaxesMenuBtn.onClick.AddListener(OnOpenTaxesMenu);
        closeTaxesMenuBtn.onClick.AddListener(OnCloseTaxesMenuHandler);
    }

    private void OnNineQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[8].SetActive(false);
        _nubiksQuests[9].SetActive(true);
        continueBtn.onClick.AddListener(OnContinueHandler);
        menuBtn.onClick.AddListener(OnMenuHandler);
        continueMenuBtn.onClick.AddListener(OnContinueMenuHandler);
    }

    private void OnTenQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        _nubiksQuests[9].SetActive(false);
        _nubiksQuests[10].SetActive(true);
    }

    private void OnElevenQuestHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        QuestsComplete = true;
        _nubiksQuests[10].SetActive(false);
    }

    private void OnMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        MenuButtonPressed?.Invoke();
        menuPanel.SetActive(true);
    }

    private void OnContinueMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        ContinueMenuButtonPressed?.Invoke();
        menuPanel.SetActive(false);
    }

    private void OnContinueHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        ContinueButtonPressed?.Invoke();
        loseMenuPanel.SetActive(false);
    }

    public void HideStructureInfo()
    {
        structurePanelHelper.Hide();
    }

    public bool GetStructureInfoVisibility()
    {

        return structurePanelHelper.gameObject.activeSelf;
    }

    private void OnConfirmActionCallback()
    {
        if (!QuestsComplete && !fiveQuestOpen)
        {
            _nubiksQuests[5].SetActive(true);
            cancleActionBtn.onClick.AddListener(OnCancleActionCallback);
            fiveQuestOpen = true;
        }
        else if(openSevenQuest)
        {
            _nubiksQuests[6].SetActive(true);
            openSevenQuest = false;
        }

        cancleActionPanel.SetActive(false);
        OnConfirmActionHandler?.Invoke();
    }

    private void OnCloseMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
    }

    private void OnCloseTaxesMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        taxesMenuPanel.SetActive(false);
    }

    private void OnDemolishHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        OnDemolishActionHandler?.Invoke();
        cancleActionPanel.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnOpenBuildMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(true);

        Debug.Log(QuestsComplete + " " + buildAreaAlreadyOpened);

        if (!QuestsComplete && !buildAreaAlreadyOpened)
        {
            _nubiksQuests[3].SetActive(false);
            _nubiksQuests[4].SetActive(true);
        }
        else if (!QuestsComplete && buildAreaAlreadyOpened)
        {
            _nubiksQuests[5].SetActive(false);
        }

        PrepareBuildMenu();
    }

    private void OnOpenTaxesMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        taxesMenuPanel.SetActive(true);
    }

    private void OnPauseHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        if (!_pause)
        {
            pauseBtn.image.sprite = _resumeIcon;
            _pause = true;
        }
        else
        {
            pauseBtn.image.sprite = _pauseIcon;
            _pause = false;
        }
        PauseButtonPressed?.Invoke();
    }

    private void PrepareBuildMenu()
    {
        CreateButtonsInPanel(zonesPanel.transform, structureRepository.GetZoneNames(), structureRepository.GetZoneCosts(), OnBuildAreaCallback);
        CreateButtonsInPanel(facilitiesPanel.transform, structureRepository.GetSingleStructureNames(), structureRepository.GetSingleStructureCosts(), OnBuildSingleStructureCallback);
        CreateButtonsInPanel(roadsPanel.transform, new List<string>() { structureRepository.GetRoadStructureName() }, new List<int>() { structureRepository.GetRoadStructureCost() }, OnBuildRoadCallback);
    }

    public void SetPopulationValue(int population)
    {
        populationValue.text = population + "";
    }

    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, List<int> placemantCost, Action<string> callback)
    {
        if (dataToShow.Count > panelTransform.childCount)
        {
            int quantityDifference = dataToShow.Count - panelTransform.childCount;
            for (int i = 0; i < quantityDifference; i++)
            {
                Instantiate(buildButtonPrefab, panelTransform);
            }
        }
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                var buttonTexts = button.GetComponentsInChildren<TextMeshProUGUI>();
                buttonTexts[0].text = dataToShow[i];
                buttonTexts[1].text = placemantCost[i].ToString() + "$";
                if (QuestsComplete || buttonTexts[0].text == "Дешёвая жилая зона")
                {
                    button.onClick.AddListener(() => callback(button.GetComponentsInChildren<TextMeshProUGUI>()[0].text));
                }
            }
        }
    }

    public void SetMoneyValue(int money)
    {
        moneyValue.text = money + "";
    }

    public void SetHappinessValue(int happiness)
    {
        happinessValue.text = (float)happiness / 100 + "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayBasicStructureInfo(StructureBaseSO data)
    {
        structurePanelHelper.DisplayBasicStructureInfo(data);
    }

    public void DisplayZoneStructureInfo(ZoneStructureSO data)
    {
        structurePanelHelper.DisplayZoneStructureInfo(data);
    }

    public void DisplayFacilitStructureInfo(SingleFacilitySO data)
    {
        structurePanelHelper.DisplayFacilityStructureInfo(data);
    }

    private bool isShopBuy = false;
    private bool isRoadBuy = false;
    private bool isWaterBuy = false;
    private bool buildAreaAlreadyOpened = false;
    private bool sixQuestOpen = false;
    private bool fiveQuestOpen = false;
    private bool openSevenQuest = false;

    private void OnBuildAreaCallback(string nameOfStructure)
    {
        if (!QuestsComplete && !buildAreaAlreadyOpened)
        {
            _nubiksQuests[4].SetActive(false);
            confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);
            CreateButtonsInPanelAgain(zonesPanel.transform, structureRepository.GetZoneNames(), structureRepository.GetZoneCosts(), OnBuildAreaCallback);
            CreateButtonsInPanelAgain(facilitiesPanel.transform, structureRepository.GetSingleStructureNames(), structureRepository.GetSingleStructureCosts(), OnBuildSingleStructureCallback);
            CreateButtonsInPanelAgain(roadsPanel.transform, new List<string>() { structureRepository.GetRoadStructureName() }, new List<int>() { structureRepository.GetRoadStructureCost() }, OnBuildRoadCallback);
            buildAreaAlreadyOpened = true;
        }
        else if(!QuestsComplete && buildAreaAlreadyOpened && !sixQuestOpen)
        {
            if(nameOfStructure == "Магазин")
                isShopBuy = true;
            if(isWaterBuy && isRoadBuy)
            {
                openSevenQuest = true;
                sixQuestOpen = true;
            }
        }

        PrepareUIForBuilding();
        OnBuildAreaHandler?.Invoke(nameOfStructure);
    }

    private void CreateButtonsInPanelAgain(Transform panelTransform, List<string> dataToShow, List<int> placemantCost, Action<string> callback)
    {
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => callback(button.GetComponentsInChildren<TextMeshProUGUI>()[0].text));
        }
    }

    private void OnBuildRoadCallback(string nameOfStructure)
    {
        if (!QuestsComplete && !sixQuestOpen)
        {
            isRoadBuy = true;
            if (isWaterBuy && isShopBuy)
            {
                openSevenQuest = true;
                sixQuestOpen = true;
            }
        }
        PrepareUIForBuilding();
        OnBuildRoadHandler?.Invoke(nameOfStructure);
    }

    private void OnBuildSingleStructureCallback(string nameOfStructure)
    {
        if (!QuestsComplete && !sixQuestOpen)
        {
            if (nameOfStructure == "Источник воды")
                isWaterBuy = true;
            if (isShopBuy && isRoadBuy)
            {
                openSevenQuest = true;
                sixQuestOpen = true;
            }
        }
        PrepareUIForBuilding();
        OnBuildSingleStructureHandler?.Invoke(nameOfStructure);
    }

    private void PrepareUIForBuilding()
    {
        cancleActionPanel.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnCancleActionCallback()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        cancleActionPanel.SetActive(false);
        OnCancleActionHandler?.Invoke();
    }

    public void AddListenerOnBuildAreaEvent(Action<string> listener)
    {
        OnBuildAreaHandler += listener;
    }

    public void RemoveListenerOnBuildAreaEvent(Action<string> listener)
    {
        OnBuildAreaHandler -= listener;
    }

    public void AddListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler += listener;
    }

    public void RemoveListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler -= listener;
    }

    public void AddListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler += listener;
    }

    public void RemoveListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler -= listener;
    }

    public void AddListenerOnCancleActionEvent(Action listener)
    {
        OnCancleActionHandler += listener;
    }

    public void RemoveListenerOnCancleActionEvent(Action listener)
    {
        OnCancleActionHandler -= listener;
    }

    public void AddListenerOnConfirmActionEvent(Action listener)
    {
        OnConfirmActionHandler += listener;
    }

    public void RemoveListenerOnConfirmActionEvent(Action listener)
    {
        OnConfirmActionHandler -= listener;
    }

    public void AddListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler += listener;
    }

    public void RemoveListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler -= listener;
    }
}
