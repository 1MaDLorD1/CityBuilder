using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject placementManagerGameObject;
    private IPlacementManager placementManager;
    public StructureRepository structureRepository;
    public IInputManager inputManger;
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

    public GameObject resourceManagerGameObject;
    private IResourceManager resourceManager;

    public WorldManager worldManager;
    private void Awake()
    {
        

#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManger = gameObject.AddComponent<InputManager>();
#endif
#if (UNITY_IOS || UNITY_ANDROID)

#endif
    }

    private void PrepareStates()
    {
        buildingManager = new BuildingManager(worldManager.Grid, placementManager, structureRepository, resourceManager);
        resourceManager.PrepareResourceManager(buildingManager);
        selectionState = new PlayerSelectionState(this, buildingManager);
        demolishState = new PlayerDemolitionState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        buildingAreaState = new PlayerBuildingZoneState(this, buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
        state = selectionState;
    }

    void Start()
    {
        placementManager = placementManagerGameObject.GetComponent<IPlacementManager>();
        placementManager.PreparePlacementManager(worldManager);
        resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
        worldManager.PrepareWorld(cellSize, width, length);
        PrepareStates();
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
