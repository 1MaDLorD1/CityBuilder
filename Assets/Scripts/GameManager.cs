using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public GameObject placementManagerGameObject;
    private PlacementManager placementManager;
    public StructureRepository structureRepository;
    public InputManager inputManger;
    public UiController uiController;
    public int width, length;
    public CameraMovement cameraMovement;
    public LayerMask inputMask;
    private BuildingManager buildingManager;
    private int cellSize = 3;

    private PlayerState state;

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerDemolitionState demolishState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingZoneState buildingAreaState;
    public PlayerState State { get => state;}
    public BuildingManager BuildingManager { get => buildingManager; set => buildingManager = value;  }
    public IResourceManager ResourceManager { get => resourceManager; }
    public bool StartAgain { get => startAgain; set => startAgain = value; }
    public bool StartFirstTime { get => startFirstTime; set => startFirstTime = value; }

    public GameObject resourceManagerGameObject;
    private IResourceManager resourceManager;

    private bool startAgain = false;

    public WorldManager worldManager;

    private bool startFirstTime = true;

    private void Awake()
    {


#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        //inputManger = gameObject.AddComponent<InputManager>();
#endif
#if (UNITY_IOS || UNITY_ANDROID)

#endif
        startFirstTime = false;
        placementManager = placementManagerGameObject.GetComponent<PlacementManager>();
        placementManager.PreparePlacementManager(worldManager);
        resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
        worldManager.PrepareWorld(cellSize, width, length);
        if (!StartAgain)
        {
            worldManager.PrepareTrees();
            buildingManager = new BuildingManager(worldManager.Grid, placementManager, structureRepository, ResourceManager);
            PrepareStates();
        }
    }

    private void PrepareStates()
    { 
        resourceManager.PrepareResourceManager(BuildingManager);
        selectionState = new PlayerSelectionState(this, BuildingManager);
        demolishState = new PlayerDemolitionState(this, BuildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, BuildingManager);
        buildingAreaState = new PlayerBuildingZoneState(this, BuildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, BuildingManager);
        state = selectionState;
    }

    void Start()
    {
        if(StartAgain)
        {
            buildingManager = new BuildingManager(worldManager.Grid, placementManager, structureRepository, ResourceManager);
            //buildingManager.grid = worldManager.Grid;
            //buildingManager.placementManager = placementManager;
            //buildingManager.resourceManager = resourceManager;
            //buildingManager.structureRepository = structureRepository;
            PrepareStates();
            buildingManager.ConfirmModificationsOnStart();
            worldManager.PrepareTreesAgain();
        }
        StartAgain = true;
        PreapreGameComponents();
        AssignInputListeners();
        AssignUiControllerListeners();
    }

    private void PreapreGameComponents()
    {
        inputManger.MouseInputMask = inputMask;
        cameraMovement.SetCameraLimits(0, width, 0, length);
    }

    private void AssignUiControllerListeners()
    {
        uiController.AddListenerOnBuildAreaEvent((structureName)=>state.OnBuildArea(structureName));
        uiController.AddListenerOnBuildSingleStructureEvent((structureName) => state.OnBuildSingleStructure(structureName));
        uiController.AddListenerOnBuildRoadEvent((structureName) => state.OnBuildRoad(structureName));
        uiController.AddListenerOnCancleActionEvent(()=>state.OnCancle());
        uiController.AddListenerOnDemolishActionEvent(()=>state.OnDemolishAction());
        uiController.AddListenerOnConfirmActionEvent(() => state.OnConfirmAction());

    }

    private void AssignInputListeners()
    {
        inputManger.AddListenerOnPointerDownEvent((position)=>state.OnInputPointerDown(position));
        inputManger.AddListenerOnPointerSecondDownEvent((position) => state.OnInputPanChange(position));
        inputManger.AddListenerOnPointerSecondUpEvent(()=>state.OnInputPanUp());
        inputManger.AddListenerOnPointerChangeEvent((position) => state.OnInputPointerChange(position));
        inputManger.AddListenerOnPointerUpEvent(() => state.OnInputPointerUp());
    }

    public void TransitionToState(PlayerState newState, string variable)
    {
        this.state = newState;
        this.state.EnterState(variable);
    }
}
