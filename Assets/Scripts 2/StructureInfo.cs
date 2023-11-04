using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureInfo
{
    public string structureType;
    public string buildingName;
    public int upkeepCost;
    public int income;
    public bool requireRoadAccess;
    public bool requireWater;
    public bool requirePower;
    public int structureRange = 1;
    //public SingleFacilitySO powerProvider = null;
    //public SingleFacilitySO waterProvider = null;
    //public RoadStructureSO roadProvider = null;
    public int maxFacilitySearchRange;
    public ZoneType zoneType;
    public int maxCustomers;
    public int upkeepPerCustomer;
    public FacilityType facilityType;
    public int singleStructureRange;
}
